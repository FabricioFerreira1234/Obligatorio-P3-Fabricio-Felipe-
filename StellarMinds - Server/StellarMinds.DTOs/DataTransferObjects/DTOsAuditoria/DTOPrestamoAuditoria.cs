using System;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria
{
    // RF11 - Préstamo realizado por un coordinador, con los datos para el listado/detalle de auditoría.
    public class DTOPrestamoAuditoria
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public string Telescopio { get; set; }
        public string Montura { get; set; }
        public string Visual { get; set; }
        public string Socio { get; set; }
        public string Coordinador { get; set; }
        public bool Atrasado { get; set; }
    }
}
