using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Models
{
    // RF05 - Devolución de préstamo (Rol Coordinador).
    public class PrestamoDevolucionViewModel
    {
        public int SocioId { get; set; }
        public bool BuscoSocio { get; set; }
        public List<UsuarioModel> Socios { get; set; } = new();
        public List<PrestamoListadoModel> Prestamos { get; set; } = new();
    }
}
