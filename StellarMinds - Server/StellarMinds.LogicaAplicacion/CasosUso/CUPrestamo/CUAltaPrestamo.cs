using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    public class CUAltaPrestamo : ICUAltaPrestamo
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioUsuario _repoUsuario;
        private readonly IRepositorioEquipo _repoEquipo;
        private readonly IRepositorioAuditoria _repoAuditoria;

        public CUAltaPrestamo(IRepositorioPrestamo repoPrestamo, IRepositorioUsuario repoUsuario, IRepositorioEquipo repoEquipo, IRepositorioAuditoria repoAuditoria)
        {
            _repoPrestamo = repoPrestamo;
            _repoUsuario = repoUsuario;
            _repoEquipo = repoEquipo;
            _repoAuditoria = repoAuditoria;
        }

        // Alta hecha por el propio socio: se resuelve el socio por su email logueado.
        public void Ejecutar(DTOAltaPrestamo p, string email)
        {
            Usuario socio = _repoUsuario.FindByEmail(email);
            if (socio == null)
                throw new PrestamoException("No se encontró el socio logueado.");

            p.UsuarioId = socio.Id;
            Registrar(p);
        }

        // Alta hecha por el Coordinador: el socio viene indicado en p.UsuarioId.
        // RF06: registra una auditoría con la acción PRESTAMO, la fecha y el coordinador que la realizó.
        public void EjecutarComoCoordinador(DTOAltaPrestamo p, string emailCoordinador)
        {
            if (p.UsuarioId <= 0)
                throw new PrestamoException("Debe seleccionar el socio que recibe el préstamo.");

            Usuario coordinador = _repoUsuario.FindByEmail(emailCoordinador);
            if (coordinador == null)
                throw new PrestamoException("No se encontró el coordinador logueado.");

            Prestamo nuevo = Registrar(p);

            _repoAuditoria.Add(new Auditoria
            {
                Accion = TipoAccionAuditoria.PRESTAMO,
                FechaHora = DateTime.Now,
                UsuarioId = coordinador.Id,
                PrestamoId = nuevo.Id
            });
        }

        // Reglas de negocio comunes a ambos flujos (RF04/RF05).
        private Prestamo Registrar(DTOAltaPrestamo p)
        {
            if (p.TelescopioId <= 0 || p.MonturaId <= 0)
                throw new PrestamoException("Un préstamo debe incluir al menos un telescopio y una montura.");

            if (_repoEquipo.FindById(p.TelescopioId) is not Telescopio telescopio)
                throw new PrestamoException("El telescopio seleccionado no existe.");

            if (_repoEquipo.FindById(p.MonturaId) is not Montura montura)
                throw new PrestamoException("La montura seleccionada no existe.");

            if (telescopio.Cantidad <= 0)
                throw new PrestamoException($"No hay disponibilidad del telescopio {telescopio.Marca} {telescopio.Modelo}.");

            if (montura.Cantidad <= 0)
                throw new PrestamoException($"No hay disponibilidad de la montura {montura.Marca} {montura.Modelo}.");

            if (montura.PesoMaximo < telescopio.Peso)
                throw new PrestamoException("La carga útil de la montura no soporta el peso del telescopio.");

            Visual visual = null;
            if (p.VisualId.HasValue && p.VisualId.Value > 0)
            {
                if (_repoEquipo.FindById(p.VisualId.Value) is not Visual visualEncontrado)
                    throw new PrestamoException("El equipo visual seleccionado no existe.");

                visual = visualEncontrado;

                if (visual.Cantidad <= 0)
                    throw new PrestamoException($"No hay disponibilidad del equipo visual {visual.Marca} {visual.Modelo}.");

                if (visual is Camara && montura.Tipo == TipoMontura.Altazimutal)
                    throw new PrestamoException("Una cámara requiere una montura Ecuatorial o Híbrida.");
            }

            telescopio.Cantidad--;
            _repoEquipo.Update(telescopio);

            montura.Cantidad--;
            _repoEquipo.Update(montura);

            if (visual != null)
            {
                visual.Cantidad--;
                _repoEquipo.Update(visual);
            }

            Prestamo nuevo = MapperPrestamo.ToPrestamo(p);
            nuevo.Estado = EstadoPrestamo.EN_PRESTAMO;
            nuevo.Validar();
            _repoPrestamo.Add(nuevo);

            return nuevo;
        }
    }
}
