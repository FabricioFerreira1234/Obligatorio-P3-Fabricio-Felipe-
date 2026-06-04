namespace StellarMinds.LogicaAplicacion.Servicios
{
    // RF07 - Configuración para consumir la API de Gemini (se lee del appsettings, sección "Gemini").
    // Se mantiene como POCO simple para no acoplar la capa de aplicación a Microsoft.Extensions.Configuration.
    public class GeminiOpciones
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Modelo { get; set; } = "gemini-2.5-flash";
    }
}
