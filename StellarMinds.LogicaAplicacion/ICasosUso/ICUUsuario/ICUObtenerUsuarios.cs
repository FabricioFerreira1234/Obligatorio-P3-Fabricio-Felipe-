using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario
{
    public interface ICUObtenerUsuarios
    {
        List<DTOUsuario> ObtenerUsuarios();
    }
}
