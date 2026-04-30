using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace StellarMinds.LogicaNegocio.ValueObjects.VOUsuario
{
    [Owned]
    public record NombreCompletoVO : IValidable
    {
        
        public string Nombre { get;  init; }
        public string Apellido { get;  init; }


        public NombreCompletoVO(string nombre, string apellido)
        {
            Nombre = nombre;
            Apellido = apellido;
        }
        public NombreCompletoVO()
        {
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                throw new UsuarioException("Nombre vacio");
            }
            if (string.IsNullOrEmpty(Apellido))
            {
                throw new UsuarioException("Apellido vacio");
            }
        }
    }
}
