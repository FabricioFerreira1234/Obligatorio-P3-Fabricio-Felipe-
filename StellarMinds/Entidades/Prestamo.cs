using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using StellarMinds.LogicaNegocio.ValueObjects.VOPrestamo;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Prestamo : IValidable
    {
        public int Id { get; set; }
        public FechaVO Fecha { get; set; }

        public int TelescopioId { get; set; }
        public Telescopio Telescopio { get; set; }

        public int MonturaId { get; set; }
        public Montura Montura { get; set; }

        public int? VisualId { get; set; }      
        public Visual Visual { get; set; }

        public EstadoPrestamo Estado { get; set; }

        public Prestamo() { }

        public Prestamo(int id, FechaVO fecha, Telescopio telescopio, Montura montura, Visual visual, EstadoPrestamo estado)
        {
            Id = id;
            Fecha = fecha;
            Telescopio = telescopio;
            Montura = montura;
            Visual = visual;
            Estado = estado;
        }


        public void Validar()
        {
            //throw new PrestamoException();
        }
    }
}
