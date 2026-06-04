using StellarMinds.LogicaNegocio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsEquipo
{
    public class DTOAltaCamara
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoSensor Sensor { get; set; }
        public decimal Resolucion { get; set; } // en micras
        public decimal PixelSize { get; set; } // en micras
    }
}
