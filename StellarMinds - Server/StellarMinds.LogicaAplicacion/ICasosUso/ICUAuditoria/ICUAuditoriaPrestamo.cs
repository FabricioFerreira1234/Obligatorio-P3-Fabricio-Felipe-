using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria
{
    // RF11 - Acciones auditadas (préstamo/devolución) de un préstamo dado.
    public interface ICUAuditoriaPrestamo
    {
        List<DTOAuditoriaItem> Ejecutar(int prestamoId);
    }
}
