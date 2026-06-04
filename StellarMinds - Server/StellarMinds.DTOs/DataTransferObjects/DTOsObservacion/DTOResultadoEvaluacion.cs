namespace StellarMinds.DTOs.DataTransferObjects.DTOsObservacion
{
    // RF07 - Respuesta de la evaluación de adecuación del equipo para el objeto celeste.
    // Indicador: IDEAL / ADECUADO / NO RECOMENDABLE. Detalle: motivo (hasta 300 caracteres) cuando no es ideal.
    public class DTOResultadoEvaluacion
    {
        public string Indicador { get; set; }
        public string Detalle { get; set; }
    }
}
