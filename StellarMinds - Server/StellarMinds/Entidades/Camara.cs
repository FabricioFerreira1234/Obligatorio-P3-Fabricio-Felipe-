using StellarMinds.LogicaNegocio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Camara : Visual
    {
        public TipoSensor Sensor { get; set; }
        public decimal Resolucion { get; set; } // en micras
        public decimal PixelSize { get; set; } // en micras
        public Camara() { }

        public Camara(int id, string marca, string modelo, int cantidad, TipoSensor sensor, decimal resolucion, decimal pixelSize)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Cantidad = cantidad;
            Sensor = sensor;
            Resolucion = resolucion;
            PixelSize = pixelSize;
        }
    }
}
