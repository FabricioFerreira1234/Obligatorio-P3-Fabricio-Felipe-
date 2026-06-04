using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUObservacion
{
    // RF10 - Lista los objetos celestes observados por los socios con la cantidad de veces
    // que fue observado cada uno, ordenados de mayor a menor. El distinct, el conteo y el orden
    // los resuelve el repositorio; los objetos nunca observados no aparecen.
    public class CURankingObjetosCelestes : ICURankingObjetosCelestes
    {
        private readonly IRepositorioObservacion _repoObservacion;

        public CURankingObjetosCelestes(IRepositorioObservacion repoObservacion)
        {
            _repoObservacion = repoObservacion;
        }

        public List<DTORankingObjetoCeleste> Ejecutar()
        {
            return _repoObservacion.ObtenerRankingObservados()
                .Select(x => MapperObjetoCeleste.ToRankingDTO(x.Objeto, x.Cantidad))
                .ToList();
        }
    }
}
