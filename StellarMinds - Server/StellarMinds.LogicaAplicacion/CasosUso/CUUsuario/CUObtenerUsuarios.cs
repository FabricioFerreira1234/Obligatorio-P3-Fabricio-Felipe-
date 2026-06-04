using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAccesoDatos.EF.Repositorios;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUUsuario
{
    public class CUObtenerUsuarios : ICUObtenerUsuarios
    {
        private readonly IRepositorioUsuario _repoUsuario;

       

        public CUObtenerUsuarios(IRepositorioUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }
        public List<DTOUsuario> ObtenerUsuarios()   
        {
            List<DTOUsuario> ret = MapperUsuario.ToListDtoPais(_repoUsuario.FindAll());
            return ret;
        }
    }
}
