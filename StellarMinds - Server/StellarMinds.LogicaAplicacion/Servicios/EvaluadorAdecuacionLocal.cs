using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;

namespace StellarMinds.LogicaAplicacion.Servicios
{
    // RF07 - Evaluador local PROVISIONAL (placeholder) hasta integrar Gemini.
    // Aplica reglas determinísticas de sentido común sobre el tipo de objeto y el tipo de observación
    // (astrofotografía si el préstamo incluye cámara, observación visual si incluye ocular).
    // Devuelve IDEAL / ADECUADO / NO RECOMENDABLE y un breve motivo (<= 300 caracteres).
    public class EvaluadorAdecuacionLocal : IServicioEvaluacionAdecuacion
    {
        private const string IDEAL = "IDEAL";
        private const string ADECUADO = "ADECUADO";
        private const string NO_RECOMENDABLE = "NO RECOMENDABLE";

        public DTOResultadoEvaluacion Evaluar(Prestamo prestamo, ObjetoCeleste objeto)
        {
            bool esAstrofotografia = prestamo.Visual is Camara;
            bool cieloProfundo = objeto.Tipo == TipoObjetoCeleste.Galaxia
                                 || objeto.Tipo == TipoObjetoCeleste.Nebulosa;

            string indicador;
            string detalle;

            if (esAstrofotografia)
            {
                // Con cámara: el cielo profundo (galaxias/nebulosas) es lo ideal para astrofotografía.
                if (cieloProfundo)
                {
                    indicador = IDEAL;
                    detalle = string.Empty;
                }
                else
                {
                    indicador = ADECUADO;
                    detalle = "Para objetos brillantes (planetas/estrellas) la cámara funciona, " +
                              "pero su mayor potencial está en objetos de cielo profundo.";
                }
            }
            else
            {
                // Observación visual (ocular): los objetos brillantes son ideales; el cielo profundo es exigente.
                if (cieloProfundo)
                {
                    indicador = NO_RECOMENDABLE;
                    detalle = "Las galaxias y nebulosas son tenues para observación visual directa; " +
                              "se recomienda una cámara para captarlas adecuadamente.";
                }
                else
                {
                    indicador = IDEAL;
                    detalle = string.Empty;
                }
            }

            return new DTOResultadoEvaluacion { Indicador = indicador, Detalle = detalle };
        }
    }
}
