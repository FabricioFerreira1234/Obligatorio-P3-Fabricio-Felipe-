using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAccesoDatos.EF.Repositorios;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUUsuario
{
    public class CUAltaUsuario : ICUAltaUsuario
    {
        private  IRepositorioUsuario _repoUsuario;

        public CUAltaUsuario(IRepositorioUsuario repoUsuario)
        {
            _repoUsuario = repoUsuario;
        }
        public void AltaUsuario(DTOAltaUsuario u)
        {
            if (_repoUsuario.FindByEmail(u.Email) is not null)
            {
                throw new UsuarioException("Ya existe un usuario registrado con ese email.");
            }

            if (u.TipoUsuario is null)
            {
                throw new UsuarioException("Debe asignar un rol al usuario.");
            }
            if (!Enum.IsDefined(typeof(TipoUsuario), u.TipoUsuario.Value))
            {
                throw new UsuarioException("El rol indicado no es válido.");
            }

            Usuario nuevo = MapperUsuario.ToUsuario(u);
            nuevo.Validar();
            _repoUsuario.Add(nuevo);
        }



    }
}


