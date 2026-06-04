using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Models
{
    // RF08 - Listado de préstamos del socio logueado en un mes y año dados.
    public class MisPrestamosViewModel
    {
        public int Mes { get; set; }
        public int Anio { get; set; }
        // Indica si el socio ya realizó una búsqueda (para mostrar resultados o el mensaje "no hay préstamos").
        public bool Busco { get; set; }
        public List<PrestamoListadoModel> Prestamos { get; set; } = new();
    }
}
