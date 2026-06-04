using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.LogicaNegocio.Entidades;

namespace StellarMinds.LogicaAplicacion.Servicios
{
    // RF07 - Abstracción del servicio que evalúa si el equipo de un préstamo es adecuado para
    // observar/fotografiar un objeto celeste. La implementación real consumirá Gemini (a integrar);
    // por ahora existe un evaluador local determinístico detrás de esta misma interfaz.
    public interface IServicioEvaluacionAdecuacion
    {
        DTOResultadoEvaluacion Evaluar(Prestamo prestamo, ObjetoCeleste objeto);
    }
}
