namespace StellarMinds.WebApp.Models
{
    // Modelos de contrato de Objeto Celeste.

    // Equivale a DTOObjetoCeleste (respuesta de GET api/Observacion/objetos).
    public class ObjetoCelesteModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Magnitud { get; set; }
    }

    // RF10 - Equivale a DTORankingObjetoCeleste (respuesta de GET api/ObjetoCeleste/ranking).
    public class RankingObjetoCelesteModel
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int CantidadObservaciones { get; set; }
    }
}
