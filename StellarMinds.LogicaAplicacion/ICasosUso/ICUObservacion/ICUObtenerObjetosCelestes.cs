using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion
{
    public interface ICUObtenerObjetosCelestes
    {
        List<DTOObjetoCeleste> Ejecutar();
    }
}
