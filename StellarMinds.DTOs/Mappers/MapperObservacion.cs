using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.LogicaNegocio.Entidades;

namespace StellarMinds.DTOs.Mappers
{
    public static class MapperObservacion
    {
        public static Observacion ToObservacion(DTOAltaObservacion dto)
        {
            return new Observacion
            {
                Fecha = dto.Fecha,
                PrestamoId = dto.PrestamoId,
                ObjetoCelesteId = dto.ObjetoCelesteId
            };
        }
    }
}
