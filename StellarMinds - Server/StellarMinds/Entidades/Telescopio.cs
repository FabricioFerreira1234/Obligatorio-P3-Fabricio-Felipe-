using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Telescopio : Equipo
    {
        public decimal Apertura { get; set; } // en milímetros
        public decimal RelacionFocal { get; set; } // en (ej. f/10, f/11, f/5)
        public decimal DistanciaFocal { get; set; } // en milímetros
        public int Peso { get; set; } // en kilogramos

        public Telescopio() { }

        public Telescopio(int id, string marca, string modelo, int cantidad, decimal apertura, decimal relacionFocal, decimal distanciaFocal, int peso)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Cantidad = cantidad;
            Apertura = apertura;
            RelacionFocal = relacionFocal;
            DistanciaFocal = distanciaFocal;
            Peso = peso;
        }
    }
}
