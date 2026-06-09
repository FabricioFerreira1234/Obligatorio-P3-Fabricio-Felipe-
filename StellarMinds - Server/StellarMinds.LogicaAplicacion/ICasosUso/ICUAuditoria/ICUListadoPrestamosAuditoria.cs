using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria
{
    // RF11 - Listado de préstamos realizados por coordinadores, filtrable por coordinador.
    public interface ICUListadoPrestamosAuditoria
    {
        List<DTOPrestamoAuditoria> Ejecutar(int? coordinadorId);
    }
}
