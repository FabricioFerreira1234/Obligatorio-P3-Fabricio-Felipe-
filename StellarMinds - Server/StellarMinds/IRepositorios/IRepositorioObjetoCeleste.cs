using StellarMinds.LogicaNegocio.Entidades;
using System.Collections.Generic;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioObjetoCeleste : IRepositorio<ObjetoCeleste>
    {
        List<ObjetoCeleste> FindAll();
        ObjetoCeleste ObtenerPorId(int id);
    }
}
