using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    // RF09 - Lista los socios (sin repetir) a los que se les prestó un telescopio dado,
    // ordenados por nombre en forma descendente. La unicidad y el orden los resuelve el repositorio.
    public class CUSociosPorTelescopio : ICUSociosPorTelescopio
    {
        private readonly IRepositorioPrestamo _repoPrestamo;

        public CUSociosPorTelescopio(IRepositorioPrestamo repoPrestamo)
        {
            _repoPrestamo = repoPrestamo;
        }

        public List<DTOSocioTelescopio> Ejecutar(int telescopioId)
        {
            if (telescopioId <= 0)
                throw new PrestamoException("Debe seleccionar un telescopio.");

            return _repoPrestamo.ObtenerSociosPorTelescopio(telescopioId)
                .Select(MapperUsuario.ToDtoSocioTelescopio)
                .ToList();
        }
    }
}
