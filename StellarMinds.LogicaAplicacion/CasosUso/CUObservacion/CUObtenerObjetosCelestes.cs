using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUObservacion
{
    public class CUObtenerObjetosCelestes : ICUObtenerObjetosCelestes
    {
        private readonly IRepositorioObjetoCeleste _repoObjeto;

        public CUObtenerObjetosCelestes(IRepositorioObjetoCeleste repoObjeto)
        {
            _repoObjeto = repoObjeto;
        }

        public List<DTOObjetoCeleste> Ejecutar()
        {
            return _repoObjeto.FindAll()
                .Select(MapperObjetoCeleste.ToDTO)
                .ToList();
        }
    }
}
