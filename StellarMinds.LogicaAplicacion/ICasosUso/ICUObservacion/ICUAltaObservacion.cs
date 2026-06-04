using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion
{
    // RF07 - Confirma el alta de la observación, guardando el resultado de la evaluación.
    public interface ICUAltaObservacion
    {
        void Ejecutar(string email, DTOAltaObservacion dto);
    }
}
