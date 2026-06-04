using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsEquipo
{
    public class DTOAltaTelescopio
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; } 
        public decimal Apertura { get; set; } // en milímetros
        public decimal RelacionFocal { get; set; } 
        public decimal DistanciaFocal { get; set; } // en milímetros
        public int Peso { get; set; } // en kilogramos
    }
}
