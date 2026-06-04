using StellarMinds.LogicaNegocio.Entidades;
using System.Collections.Generic;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioObservacion : IRepositorio<Observacion>
    {
        List<Observacion> FindAll();
        // RF10 - Objetos celestes efectivamente observados con su cantidad de observaciones,
        // ordenados de mayor a menor. Los nunca observados no se incluyen.
        List<(ObjetoCeleste Objeto, int Cantidad)> ObtenerRankingObservados();
    }
}
