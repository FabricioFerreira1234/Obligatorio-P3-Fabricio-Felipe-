using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUAuditoria
{
    // RF11 - Información completa de un préstamo. El coordinador es el que figura en la acción
    // PRESTAMO de la auditoría (quien dio el alta).
    public class CUDetallePrestamo : ICUDetallePrestamo
    {
        private readonly IRepositorioPrestamo _repoPrestamo;
        private readonly IRepositorioAuditoria _repoAuditoria;

        public CUDetallePrestamo(IRepositorioPrestamo repoPrestamo, IRepositorioAuditoria repoAuditoria)
        {
            _repoPrestamo = repoPrestamo;
            _repoAuditoria = repoAuditoria;
        }

        public DTOPrestamoAuditoria Ejecutar(int prestamoId)
        {
            Prestamo prestamo = _repoPrestamo.ObtenerDetalle(prestamoId);
            if (prestamo == null)
                throw new PrestamoException("El préstamo seleccionado no existe.");

            Auditoria alta = _repoAuditoria.ObtenerPorPrestamo(prestamoId)
                .FirstOrDefault(a => a.Accion == TipoAccionAuditoria.PRESTAMO);

            string coordinador = alta?.Usuario?.NombreCompleto == null
                ? null
                : $"{alta.Usuario.NombreCompleto.Nombre} {alta.Usuario.NombreCompleto.Apellido}";

            return MapperAuditoria.ToDTOPrestamoAuditoria(prestamo, coordinador);
        }
    }
}
