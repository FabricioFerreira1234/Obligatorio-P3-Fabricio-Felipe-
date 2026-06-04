using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsUsuario
{
    public class DTOAltaUsuario
    {
        public NombreCompletoVO? NombreCompleto { get; set; }
        public DireccionVO? Direccion { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string  Pass { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }


        public DTOAltaUsuario()
        {
            NombreCompleto = new NombreCompletoVO();
            Direccion = new DireccionVO();
        }

    }
}
