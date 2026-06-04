using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    public class CUListadoPrestamosSocio : ICUListadoPrestamosSocio
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioUsuario _repoUsuario;

        public CUListadoPrestamosSocio(IRepositorioPrestamo repoPrestamo, IRepositorioUsuario repoUsuario)
        {
            _repoPrestamo = repoPrestamo;
            _repoUsuario = repoUsuario;
        }

        public List<DTOPrestamoListado> Ejecutar(string email, int mes, int anio)
        {
            if (mes < 1 || mes > 12)
                throw new PrestamoException("El mes debe estar entre 1 y 12.");

            Usuario socio = _repoUsuario.FindByEmail(email);
            if (socio == null)
                throw new PrestamoException("No se encontró el socio logueado.");

            return _repoPrestamo.ObtenerPorSocioYMes(socio.Id, mes, anio)
                .Select(MapperPrestamo.ToDTOListado)
                .ToList();
        }
    }
}
