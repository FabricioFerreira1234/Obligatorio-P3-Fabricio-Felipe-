using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using StellarMinds.LogicaNegocio.Entidades;

namespace StellarMinds.DTOs.Mappers
{
    public static class MapperObjetoCeleste
    {
        public static DTOObjetoCeleste ToDTO(ObjetoCeleste o)
        {
            return new DTOObjetoCeleste
            {
                Id = o.Id,
                Nombre = o.Nombre,
                Tipo = o.Tipo.ToString(),
                Magnitud = o.Magnitud
            };
        }

        // RF10 - Fila del ranking: el objeto celeste observado y la cantidad de veces que fue observado.
        public static DTORankingObjetoCeleste ToRankingDTO(ObjetoCeleste o, int cantidad)
        {
            return new DTORankingObjetoCeleste
            {
                Nombre = o.Nombre,
                Tipo = o.Tipo.ToString(),
                CantidadObservaciones = cantidad
            };
        }
    }
}
