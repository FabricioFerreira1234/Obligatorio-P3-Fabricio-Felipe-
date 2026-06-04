using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;

namespace StellarMinds.WebApp.Models
{
    // Forma del cuerpo de error que devuelve la WebAPI: { "message": "..." }.
    public class MensajeApi
    {
        public string Message { get; set; }
    }

    // Lectura del mensaje de error de una respuesta fallida de la API.
    public static class RespuestaApi
    {
        public static string LeerError(HttpResponseMessage respuesta)
        {
            try
            {
                string cuerpo = ClienteHttpAuxiliar.ObtenerBody(respuesta);
                MensajeApi error = JsonConvert.DeserializeObject<MensajeApi>(cuerpo);
                if (!string.IsNullOrWhiteSpace(error?.Message)) return error.Message;
            }
            catch
            {
                // El cuerpo no era el JSON de error esperado: se usa el mensaje por defecto.
            }
            return $"La API respondió {(int)respuesta.StatusCode}.";
        }
    }
}
