using System;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria
{
    // RF11 - Una acción auditada de un préstamo: acción (PRESTAMO/DEVOLUCION), fecha y coordinador que la realizó.
    public class DTOAuditoriaItem
    {
        public int PrestamoId { get; set; }
        public string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Coordinador { get; set; }
        public string CoordinadorEmail { get; set; }
    }
}
