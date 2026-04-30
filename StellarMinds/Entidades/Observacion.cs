using StellarMinds.LogicaNegocio.InterfacesDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Observacion : IValidable
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        //public Prestamo Prestamo { get; set; }
        //public ObjetoCeleste ObjetoCeleste { get; set; }
        public string ResultadoIA { get; set; }
        public string MotivoIA { get; set; }

        public Observacion(int id, DateTime fecha,/*Prestamo p, ObjetoCeleste oc,*/ string resultadoIA, string motivoIA)
        {
            Id = id;
            Fecha = fecha;
            //Prestamo = p;
            //ObjetoCeleste = oc;
            ResultadoIA = resultadoIA;
            MotivoIA = motivoIA;
        }   
        

        public void Validar()
        {
            //throw new NotImplementedException();
        }
    }
}
