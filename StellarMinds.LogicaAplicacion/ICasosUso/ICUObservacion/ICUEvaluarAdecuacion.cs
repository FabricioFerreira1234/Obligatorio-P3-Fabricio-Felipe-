using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion
{
    // RF07 - Evalúa (sin guardar) si el equipo del préstamo es adecuado para el objeto celeste.
    public interface ICUEvaluarAdecuacion
    {
        DTOResultadoEvaluacion Ejecutar(string email, DTOAltaObservacion dto);
    }
}
