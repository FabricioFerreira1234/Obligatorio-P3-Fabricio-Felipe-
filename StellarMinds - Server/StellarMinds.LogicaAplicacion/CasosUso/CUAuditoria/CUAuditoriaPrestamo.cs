using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUAuditoria
{
    // RF11 - Devuelve las acciones auditadas (préstamo y devolución) de un préstamo, con fecha y coordinador.
    public class CUAuditoriaPrestamo : ICUAuditoriaPrestamo
    {
        private readonly IRepositorioAuditoria _repoAuditoria;

        public CUAuditoriaPrestamo(IRepositorioAuditoria repoAuditoria)
        {
            _repoAuditoria = repoAuditoria;
        }

        public List<DTOAuditoriaItem> Ejecutar(int prestamoId)
        {
            if (prestamoId <= 0)
                throw new PrestamoException("Debe indicar un préstamo válido.");

            return _repoAuditoria.ObtenerPorPrestamo(prestamoId)
                .Select(MapperAuditoria.ToDTOItem)
                .ToList();
        }
    }
}
