using StellarMinds.WebApp.Enums;
using System.Net.Http.Json;

namespace StellarMinds.WebApp.Auxiliar
{
    // Cliente HTTP auxiliar: centraliza TODAS las llamadas a la WebAPI (misma idea que el ejemplo del curso).
    // Acá es donde se le pasa todo: la URL, el verbo, el objeto (cuerpo) y el token.
    //  - Si viene token  -> se envía en el header Authorization como "Bearer {token}" (flujo seguro).
    //  - Si viene un obj  -> se serializa a JSON y viaja como cuerpo (POST/PUT).
    // Devuelve el HttpResponseMessage crudo; el cuerpo se lee aparte con ObtenerBody().
    public class ClienteHttpAuxiliar
    {
        // URL base de la WebAPI. Los controllers arman la URL completa: UrlBase + "Recurso".
        public const string UrlBase = "https://localhost:7011/api/";

        public static HttpResponseMessage EnviarSolicitud(
            string url, VerbosHttp verbo, object obj = null, string token = null)
        {
            HttpClient cliente = new HttpClient();
            Task<HttpResponseMessage> tarea = null;

            // Flujo seguro: el token JWT (guardado en sesión tras el login) viaja en cada request.
            if (!string.IsNullOrWhiteSpace(token))
            {
                cliente.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }

            switch (verbo)
            {
                case VerbosHttp.GET:
                    tarea = cliente.GetAsync(url);
                    break;
                case VerbosHttp.POST:
                    tarea = cliente.PostAsJsonAsync(url, obj);
                    break;
                case VerbosHttp.PUT:
                    tarea = cliente.PutAsJsonAsync(url, obj);
                    break;
                case VerbosHttp.DELETE:
                    tarea = cliente.DeleteAsync(url);
                    break;
                default:
                    throw new Exception("VERBO NO VALIDO");
            }

            tarea.Wait();
            return tarea.Result;
        }

        public static string ObtenerBody(HttpResponseMessage respuesta)
        {
            HttpContent body = respuesta.Content;
            Task<string> tarea = body.ReadAsStringAsync();
            tarea.Wait();
            return tarea.Result;
        }
    }
}
