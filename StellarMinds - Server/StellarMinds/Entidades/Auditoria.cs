using StellarMinds.LogicaNegocio.Enumeraciones;
using System;

namespace StellarMinds.LogicaNegocio.Entidades
{
    // RF06 - Auditoría automática: registra la acción (préstamo/devolución), la fecha y el coordinador que la realizó.
    public class Auditoria
    {
        public int Id { get; set; }
        public TipoAccionAuditoria Accion { get; set; }
        public DateTime FechaHora { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int PrestamoId { get; set; }
        public Prestamo Prestamo { get; set; }
    }
}
