namespace StellarMinds.WebApp.Models
{
    // Modelos de contrato de Observación (RF07).

    // Cuerpo de POST api/Observacion y api/Observacion/evaluar.
    public class AltaObservacionModel
    {
        public int PrestamoId { get; set; }
        public int ObjetoCelesteId { get; set; }
        public DateTime Fecha { get; set; }
    }

    // Respuesta de la evaluación de adecuación.
    public class ResultadoEvaluacionModel
    {
        public string Indicador { get; set; }
        public string Detalle { get; set; }
    }
}
