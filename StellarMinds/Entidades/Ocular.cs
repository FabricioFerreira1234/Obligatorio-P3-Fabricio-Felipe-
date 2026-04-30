using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Ocular : Visual
    {
        public decimal Diametro { get; set; } // en milímetros
        public decimal AnguloVisual { get; set; } // en grados

        public Ocular() { }
        public Ocular(int id, string marca, string modelo, int cantidad, decimal diametro, decimal anguloVisual)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Cantidad = cantidad;
            Diametro = diametro;
            AnguloVisual = anguloVisual;
        }
    }
}
