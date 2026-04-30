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

    public record DireccionVO : IValidable
    {
        public string Calle1 { get;  init; }
        public string Calle2 { get;  init; }

        public DireccionVO() { }
        public DireccionVO(string calle1, string calle2)
        {
            Calle1 = calle1;
            Calle2 = calle2;
        }

        public void Validar()
        {
            if (string.IsNullOrEmpty(Calle1))
            {
                throw new UsuarioException("Calle 1 vacia");
            }
    
            if (string.IsNullOrEmpty(Calle2))
            {
                throw new UsuarioException("Calle 2 vacia");
            }
        }

    }
}
