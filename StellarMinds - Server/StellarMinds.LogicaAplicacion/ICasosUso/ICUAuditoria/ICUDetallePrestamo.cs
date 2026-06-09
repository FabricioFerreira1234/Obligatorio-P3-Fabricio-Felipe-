using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria
{
    // RF11 - Información completa de un préstamo (link de detalles).
    public interface ICUDetallePrestamo
    {
        DTOPrestamoAuditoria Ejecutar(int prestamoId);
    }
}
