using StellarMinds.LogicaNegocio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Montura : Equipo
    {
        public TipoMontura Tipo { get; set; }
        public int PesoMaximo { get; set; } // en kilogramos
        public bool Goto { get; set; } 

        public Montura() { }
        public Montura(int id, string marca, string modelo, int cantidad, TipoMontura tipo, int pesoMaximo, bool gotoSistema)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Cantidad = cantidad;
            Tipo = tipo;
            PesoMaximo = pesoMaximo;
            Goto = gotoSistema;
        }
    }
}
