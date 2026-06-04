using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Excepciones;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace StellarMinds.LogicaAplicacion.Servicios
{
    // RF07 - Implementación REAL del evaluador de adecuación: consume la API de Gemini de Google.
    // Arma el mismo JSON que se probó en Postman (datos del telescopio + cámara u ocular + objeto celeste),
    // lo envía al endpoint generateContent con structured output (responseSchema) y obtiene de vuelta
    // un { indicador, detalle } garantizado. Reemplaza a EvaluadorAdecuacionLocal detrás de la misma interfaz.
    public class EvaluadorAdecuacionGemini : IServicioEvaluacionAdecuacion
    {
        private const string UrlBase = "https://generativelanguage.googleapis.com/v1beta/models/";

        private readonly HttpClient _http;
        private readonly GeminiOpciones _opciones;

        private static readonly JsonSerializerOptions _opcionesJson = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public EvaluadorAdecuacionGemini(HttpClient http, GeminiOpciones opciones)
        {
            _http = http;
            _opciones = opciones;
            _http.DefaultRequestHeaders.Remove("x-goog-api-key");
            _http.DefaultRequestHeaders.Add("x-goog-api-key", _opciones.ApiKey);
        }

        public DTOResultadoEvaluacion Evaluar(Prestamo prestamo, ObjetoCeleste objeto)
        {
            string cuerpo = ConstruirCuerpo(prestamo, objeto);
            string respuestaCruda = EnviarAGemini(cuerpo);
            return ParsearRespuesta(respuestaCruda);
        }

        // Arma el body de la petición: el prompt con el JSON de la letra + el responseSchema que obliga
        // a Gemini a contestar exactamente { indicador, detalle }.
        private string ConstruirCuerpo(Prestamo prestamo, ObjetoCeleste objeto)
        {
            var datos = new Dictionary<string, object>
            {
                ["telescopio"] = new
                {
                    apertura_mm = prestamo.Telescopio.Apertura,
                    focal_mm = prestamo.Telescopio.DistanciaFocal,
                    relacion_focal = prestamo.Telescopio.RelacionFocal
                },
                ["objeto_celeste"] = new
                {
                    nombre = objeto.Nombre,
                    tipo = objeto.Tipo.ToString()
                }
            };

            // La cámara y el ocular son excluyentes: si hay cámara es astrofotografía, si hay ocular es visual.
            if (prestamo.Visual is Camara camara)
            {
                datos["camara"] = new
                {
                    sensor = camara.Sensor.ToString(),
                    resolucion_px = camara.Resolucion,
                    pixel_size_um = camara.PixelSize
                };
            }
            else if (prestamo.Visual is Ocular ocular)
            {
                datos["ocular"] = new
                {
                    diametro_mm = ocular.Diametro,
                    campo_aparente_grados = ocular.AnguloVisual
                };
            }

            string datosJson = JsonSerializer.Serialize(datos);

            string prompt =
                "Eres un experto en astronomía y equipos de observación. Evalúa si el siguiente equipo es " +
                "adecuado para observar o fotografiar el objeto celeste indicado. Si el préstamo incluye cámara " +
                "asume astrofotografía; si incluye ocular asume observación visual directa. Responde con un " +
                "indicador IDEAL, ADECUADO o NO RECOMENDABLE y un detalle breve (máximo 300 caracteres) que " +
                "explique el motivo solo cuando no sea IDEAL. Datos: " + datosJson;

            var cuerpo = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                generationConfig = new
                {
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            indicador = new
                            {
                                type = "STRING",
                                @enum = new[] { "IDEAL", "ADECUADO", "NO RECOMENDABLE" }
                            },
                            detalle = new { type = "STRING" }
                        },
                        required = new[] { "indicador", "detalle" }
                    }
                }
            };

            return JsonSerializer.Serialize(cuerpo);
        }

        // Hace el POST al endpoint generateContent y devuelve el JSON crudo de la respuesta.
        private string EnviarAGemini(string cuerpo)
        {
            string url = UrlBase + _opciones.Modelo + ":generateContent";

            using var solicitud = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(cuerpo, Encoding.UTF8, "application/json")
            };

            using HttpResponseMessage respuesta = _http.Send(solicitud);
            using var lector = new StreamReader(respuesta.Content.ReadAsStream());
            string json = lector.ReadToEnd();

            if (!respuesta.IsSuccessStatusCode)
                throw new ObservacionException("No se pudo evaluar la adecuación con el servicio de IA (Gemini).");

            return json;
        }

        // Extrae el texto generado (candidates[0].content.parts[0].text), que a su vez es el JSON
        // { indicador, detalle }, y lo mapea al DTO de resultado.
        private DTOResultadoEvaluacion ParsearRespuesta(string respuestaCruda)
        {
            try
            {
                using JsonDocument documento = JsonDocument.Parse(respuestaCruda);

                string? texto = documento.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                if (string.IsNullOrWhiteSpace(texto))
                    throw new ObservacionException("El servicio de IA (Gemini) devolvió una respuesta vacía.");

                DTOResultadoEvaluacion? resultado =
                    JsonSerializer.Deserialize<DTOResultadoEvaluacion>(texto, _opcionesJson);

                if (resultado == null || string.IsNullOrWhiteSpace(resultado.Indicador))
                    throw new ObservacionException("El servicio de IA (Gemini) devolvió una respuesta inválida.");

                // Salvaguarda: la letra limita el detalle a 300 caracteres.
                if (!string.IsNullOrEmpty(resultado.Detalle) && resultado.Detalle.Length > 300)
                    resultado.Detalle = resultado.Detalle.Substring(0, 300);

                return resultado;
            }
            catch (ObservacionException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ObservacionException("No se pudo interpretar la respuesta del servicio de IA (Gemini).");
            }
        }
    }
}
