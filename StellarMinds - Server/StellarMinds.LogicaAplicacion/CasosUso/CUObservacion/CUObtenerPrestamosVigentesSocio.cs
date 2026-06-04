using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUObservacion
{
    // RF07 - Lista los préstamos del socio logueado que están EN PRÉSTAMO y aún vigentes
    // (su fecha de fin no pasó), que son los candidatos sobre los que se puede planificar una observación.
    public class CUObtenerPrestamosVigentesSocio : ICUObtenerPrestamosVigentesSocio
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioUsuario _repoUsuario;

        public CUObtenerPrestamosVigentesSocio(IRepositorioPrestamo repoPrestamo, IRepositorioUsuario repoUsuario)
        {
            _repoPrestamo = repoPrestamo;
            _repoUsuario = repoUsuario;
        }

        public List<DTOPrestamoListado> Ejecutar(string email)
        {
            Usuario socio = _repoUsuario.FindByEmail(email);
            if (socio == null)
                throw new ObservacionException("No se encontró el socio logueado.");

            return _repoPrestamo.ObtenerPorSocioYEstado(socio.Id, EstadoPrestamo.EN_PRESTAMO)
                .Where(p => p.Fecha.Fin.Date >= DateTime.Now.Date)
                .Select(MapperPrestamo.ToDTOListado)
                .ToList();
        }
    }
}
