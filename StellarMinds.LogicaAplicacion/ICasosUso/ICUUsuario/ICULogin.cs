using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario
{
    public interface ICULogin
    {
        Usuario Login(string email, string pass);
    }
}
