using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Models
{
    public class PrestamoCreateViewModel
    {
        public int SocioId { get; set; }
        public int TelescopioId { get; set; }
        public int MonturaId { get; set; }
        public int? VisualId { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Today;
        public DateTime FechaFin { get; set; } = DateTime.Today.AddDays(7);

        public List<UsuarioModel> Socios { get; set; } = new();
        public List<TelescopioModel> Telescopios { get; set; } = new();
        public List<MonturaModel> Monturas { get; set; } = new();
        public List<CamaraModel> Camaras { get; set; } = new();
        public List<OcularModel> Oculares { get; set; } = new();
    }
}
