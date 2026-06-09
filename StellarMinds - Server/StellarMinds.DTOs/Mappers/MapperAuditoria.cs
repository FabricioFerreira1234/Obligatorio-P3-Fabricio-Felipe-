using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using StellarMinds.LogicaNegocio.Entidades;

namespace StellarMinds.DTOs.Mappers
{
    // RF11 - Mapea las auditorías y los préstamos auditados a sus DTOs.
    public static class MapperAuditoria
    {
        // Una fila del listado de auditoría a partir del registro de alta (PRESTAMO), que trae
        // tanto el préstamo (con sus equipos y socio) como el coordinador que lo realizó.
        public static DTOPrestamoAuditoria ToDTOPrestamoAuditoria(Auditoria alta)
        {
            return ToDTOPrestamoAuditoria(alta.Prestamo, NombreCompleto(alta.Usuario));
        }

        public static DTOPrestamoAuditoria ToDTOPrestamoAuditoria(Prestamo p, string coordinador)
        {
            return new DTOPrestamoAuditoria
            {
                Id = p.Id,
                FechaInicio = p.Fecha.Inicio,
                FechaFin = p.Fecha.Fin,
                Estado = p.Estado.ToString(),
                Telescopio = p.Telescopio == null ? null : $"{p.Telescopio.Marca} {p.Telescopio.Modelo}",
                Montura = p.Montura == null ? null : $"{p.Montura.Marca} {p.Montura.Modelo}",
                Visual = p.Visual == null ? null : $"{p.Visual.Marca} {p.Visual.Modelo}",
                Socio = NombreCompleto(p.Usuario),
                Coordinador = coordinador,
                Atrasado = p.EstaAtrasado()
            };
        }

        public static DTOAuditoriaItem ToDTOItem(Auditoria a)
        {
            return new DTOAuditoriaItem
            {
                PrestamoId = a.PrestamoId,
                Accion = a.Accion.ToString(),
                FechaHora = a.FechaHora,
                Coordinador = NombreCompleto(a.Usuario),
                CoordinadorEmail = a.Usuario?.Email
            };
        }

        private static string NombreCompleto(Usuario u) =>
            u?.NombreCompleto == null ? null : $"{u.NombreCompleto.Nombre} {u.NombreCompleto.Apellido}";
    }
}
