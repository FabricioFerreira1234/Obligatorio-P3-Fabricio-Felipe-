using System;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo
{
    public class DTOPrestamoListado
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
}
