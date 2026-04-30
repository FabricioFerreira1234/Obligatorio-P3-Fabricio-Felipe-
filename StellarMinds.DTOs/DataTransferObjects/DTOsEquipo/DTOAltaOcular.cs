using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsEquipo
{
    public class DTOAltaOcular
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Diametro { get; set; }
        public decimal AnguloVisual { get; set; }
    }
}
