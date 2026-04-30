using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.Mappers
{
    public class MapperUsuario
    {
        public static Usuario ToUsuario(DTOAltaUsuario dto)
        {
            Usuario retorno = new Usuario();
            retorno.NombreCompleto = dto.NombreCompleto;    
            retorno.Direccion = dto.Direccion;
            retorno.Telefono = dto.Telefono;
            retorno.Email = dto.Email;
            retorno.Username = dto.Username;
            retorno.Pass = dto.Pass;
            retorno.TipoUsuario = (LogicaNegocio.Enumeraciones.TipoUsuario)dto.TipoUsuario;
         
            return retorno;
        }

        public static List<DTOUsuario>ToListDtoPais(List<Usuario> listUsuarios)
        {
            List<DTOUsuario> retorno = new List<DTOUsuario>();
            foreach (Usuario u in listUsuarios)
            {
                DTOUsuario dto = ToDtoUsuario(u);
                retorno.Add(dto);
            }
            return retorno;
        }

        public static DTOUsuario ToDtoUsuario(Usuario p)
        {
            DTOUsuario retorno = new DTOUsuario();
            retorno.NombreCompleto = p.NombreCompleto;
            retorno.Direccion = p.Direccion;
            retorno.Telefono = p.Telefono;
            retorno.Email = p.Email;
            retorno.Username = p.Username;
            retorno.Pass = p.Pass;
            retorno.TipoUsuario = p.TipoUsuario;
            return retorno;

        }
    }
}
