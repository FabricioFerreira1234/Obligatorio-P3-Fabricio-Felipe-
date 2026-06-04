namespace StellarMinds.WebApp.Models.Api
{
    // Modelos de contrato de Préstamo.

    // Equivale al value object FechaVO del servidor (mismo shape JSON: Inicio / Fin).
    public class FechaModel
    {
        public DateTime Inicio { get; set; }
        public DateTime Fin { get; set; }
    }

    // RF08 - Equivale a DTOPrestamoListado (respuesta de los listados de préstamos).
    public class PrestamoListadoModel
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; }
        public string Telescopio { get; set; }
        public string Montura { get; set; }
        public string Visual { get; set; }
        public bool Atrasado { get; set; }
    }

    // RF04 - Equivale a DTOAltaPrestamo (cuerpo de POST api/Prestamo y api/Prestamo/coordinador).
    public class AltaPrestamoModel
    {
        public int Id { get; set; }
        public FechaModel Fecha { get; set; }
        public int TelescopioId { get; set; }
        public int MonturaId { get; set; }
        public int? VisualId { get; set; }
        public int UsuarioId { get; set; }
        public EstadoPrestamo Estado { get; set; }
    }
}
