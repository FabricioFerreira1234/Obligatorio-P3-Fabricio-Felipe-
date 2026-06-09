namespace StellarMinds.WebApp.Models
{
    // RF11 - Listado de préstamos auditados, filtrable por coordinador.
    public class AuditoriaIndexViewModel
    {
        public int? CoordinadorId { get; set; }
        public List<UsuarioModel> Coordinadores { get; set; } = new();
        public List<PrestamoAuditoriaModel> Prestamos { get; set; } = new();
    }

    // RF11 - Acciones auditadas de un préstamo.
    public class AuditoriaPrestamoViewModel
    {
        public int PrestamoId { get; set; }
        public List<AuditoriaItemModel> Items { get; set; } = new();
    }

    // RF11 - Detalle completo de un préstamo.
    public class PrestamoDetalleViewModel
    {
        public PrestamoAuditoriaModel Prestamo { get; set; } = new();
    }
}
