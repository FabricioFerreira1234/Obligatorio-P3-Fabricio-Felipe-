using Newtonsoft.Json;

namespace StellarMinds.WebApp.Auxiliar
{
    // Helper de lectura de la respuesta de la API (complementa a ClienteHttpAuxiliar, sin ensuciar sus 2 métodos).
    public static class RespuestaApi
    {
        // Extrae el mensaje de error { "message": "..." } que devuelve la API, o un texto por defecto.
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

    public class MensajeApi
    {
        public string Message { get; set; }
    }
}
