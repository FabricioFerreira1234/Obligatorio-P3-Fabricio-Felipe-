using StellarMinds.LogicaNegocio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsEquipo
{
    public class DTOAltaMontura
    {
        public int Id { get; set; }

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoMontura Tipo { get; set; }
        public int PesoMaximo { get; set; } // en kilogramos
        public bool Goto { get; set; }


    }
}
