using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    // RF05 - Lista los préstamos EN PRÉSTAMO de un socio para que el Coordinador elija cuál devolver.
    public class CUListadoPrestamosEnPrestamoSocio : ICUListadoPrestamosEnPrestamoSocio
    {
        private readonly IRepositorioPrestamo _repoPrestamo;

        public CUListadoPrestamosEnPrestamoSocio(IRepositorioPrestamo repoPrestamo)
        {
            _repoPrestamo = repoPrestamo;
        }

        public List<DTOPrestamoListado> Ejecutar(int socioId)
        {
            if (socioId <= 0)
                throw new PrestamoException("Debe seleccionar un socio.");

            return _repoPrestamo.ObtenerPorSocioYEstado(socioId, EstadoPrestamo.EN_PRESTAMO)
                .Select(MapperPrestamo.ToDTOListado)
                .ToList();
        }
    }
}
