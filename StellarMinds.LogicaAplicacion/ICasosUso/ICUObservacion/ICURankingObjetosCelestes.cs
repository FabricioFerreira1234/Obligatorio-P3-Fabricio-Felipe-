using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion
{
    // RF10 - Ranking de objetos celestes observados (nombre, tipo y cantidad), orden desc por cantidad.
    public interface ICURankingObjetosCelestes
    {
        List<DTORankingObjetoCeleste> Ejecutar();
    }
}
