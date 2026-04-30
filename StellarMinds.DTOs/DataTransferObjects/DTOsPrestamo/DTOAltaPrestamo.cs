using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.ValueObjects.VOPrestamo;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo
{
    public class DTOAltaPrestamo
    {
        
         public int Id { get; set; }
        public FechaVO Fecha { get; set; }
        public int TelescopioId { get; set; }
        public int MonturaId { get; set; }
        public int VisualId { get; set; }
        public EstadoPrestamo Estado { get; set; }  


    }
}
