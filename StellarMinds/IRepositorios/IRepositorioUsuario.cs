using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioUsuario
    {
         void Add(Usuario u);
        List<Usuario> FindAll();
        Usuario FindByEmail(string e);
        void Delete(Usuario u);
      


    }
}
