using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public abstract class Equipo : IValidable
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public Equipo() { }

        public Equipo(int id, string marca, string modelo, int cantidad)
        {
            Id = id;
            Marca = marca;
            Modelo = modelo;
            Cantidad = cantidad;
        }


        public void Validar()
        {
           // throw new EquipoException();
        }
    }
}
