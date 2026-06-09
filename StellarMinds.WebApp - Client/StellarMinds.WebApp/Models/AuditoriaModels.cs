namespace StellarMinds.WebApp.Models
{
    // RF11 - Modelos de contrato de Auditoría (equivalen a los DTOs del servidor).

    // Equivale a DTOPrestamoAuditoria (préstamo realizado por un coordinador).
    public class PrestamoAuditoriaModel
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

    // Equivale a DTOAuditoriaItem (una acción auditada de un préstamo).
    public class AuditoriaItemModel
    {
        public int PrestamoId { get; set; }
        public string Accion { get; set; }
        public DateTime FechaHora { get; set; }
        public string Coordinador { get; set; }
        public string CoordinadorEmail { get; set; }
    }
}
