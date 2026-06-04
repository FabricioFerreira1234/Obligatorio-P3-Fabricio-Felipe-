using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    // RF05 - Devolución de préstamo: pasa el préstamo a DEVUELTO y reintegra la cantidad de los equipos.
    public class CUDevolverPrestamo : ICUDevolverPrestamo
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioEquipo _repoEquipo;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioAuditoria _repoAuditoria;

        public CUDevolverPrestamo(IRepositorioPrestamo repoPrestamo, IRepositorioEquipo repoEquipo, IRepositorioUsuario repoUsuario, IRepositorioAuditoria repoAuditoria)
        {
            _repoPrestamo = repoPrestamo;
            _repoEquipo = repoEquipo;
            _repoUsuario = repoUsuario;
            _repoAuditoria = repoAuditoria;
        }

        // RF06: registra una auditoría con la acción DEVOLUCION, la fecha y el coordinador que la realizó.
        public void Ejecutar(int prestamoId, string emailCoordinador)
        {
            Prestamo prestamo = _repoPrestamo.ObtenerPorId(prestamoId);
            if (prestamo == null)
                throw new PrestamoException("El préstamo seleccionado no existe.");

            if (prestamo.Estado != EstadoPrestamo.EN_PRESTAMO)
                throw new PrestamoException("El préstamo ya fue devuelto.");

            Usuario coordinador = _repoUsuario.FindByEmail(emailCoordinador);
            if (coordinador == null)
                throw new PrestamoException("No se encontró el coordinador logueado.");

            ReintegrarEquipo(prestamo.TelescopioId);
            ReintegrarEquipo(prestamo.MonturaId);
            if (prestamo.VisualId.HasValue)
                ReintegrarEquipo(prestamo.VisualId.Value);

            prestamo.Estado = EstadoPrestamo.DEVUELTO;
            _repoPrestamo.Update(prestamo);

            _repoAuditoria.Add(new Auditoria
            {
                Accion = TipoAccionAuditoria.DEVOLUCION,
                FechaHora = DateTime.Now,
                UsuarioId = coordinador.Id,
                PrestamoId = prestamo.Id
            });
        }

        private void ReintegrarEquipo(int equipoId)
        {
            Equipo equipo = _repoEquipo.FindById(equipoId);
            if (equipo == null)
                return;

            equipo.Cantidad++;
            _repoEquipo.Update(equipo);
        }
    }
}
