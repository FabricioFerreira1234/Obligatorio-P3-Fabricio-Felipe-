using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Models
{
    // RF07 - Alta de observación (Rol Socio): selección + resultado de la evaluación de adecuación.
    public class ObservacionCreateViewModel
    {
        public int PrestamoId { get; set; }
        public int ObjetoCelesteId { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Today;

        public List<PrestamoListadoModel> Prestamos { get; set; } = new();
        public List<ObjetoCelesteModel> Objetos { get; set; } = new();

        // Resultado de la evaluación (se completa al apretar "Ver evaluación").
        public bool Evaluado { get; set; }
        public string Indicador { get; set; }
        public string Detalle { get; set; }
    }
}
