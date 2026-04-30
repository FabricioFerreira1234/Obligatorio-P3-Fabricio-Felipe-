using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios; 
using System;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUUsuario
{
    public class CUEliminar : ICUEliminar
    {
        private  IRepositorioUsuario _repoUsuario; 

        public CUEliminar(IRepositorioUsuario repoUsuario) 
        {
            _repoUsuario = repoUsuario;
        }

        public void Eliminar(string email)
        {
            Usuario buscado = _repoUsuario.FindByEmail(email);
            if (buscado is not null)
            {
                _repoUsuario.Delete(buscado);
            }
        }
    }
}