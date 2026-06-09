using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUAuditoria
{
    // RF11 - Lista los préstamos dados de alta por un coordinador (o todos), tomando el registro
    // de auditoría de tipo PRESTAMO de cada préstamo.
    public class CUListadoPrestamosAuditoria : ICUListadoPrestamosAuditoria
    {
        private readonly IRepositorioAuditoria _repoAuditoria;

        public CUListadoPrestamosAuditoria(IRepositorioAuditoria repoAuditoria)
        {
            _repoAuditoria = repoAuditoria;
        }

        public List<DTOPrestamoAuditoria> Ejecutar(int? coordinadorId)
        {
            return _repoAuditoria.ObtenerAltas(coordinadorId)
                .Select(a => MapperAuditoria.ToDTOPrestamoAuditoria(a))
                .ToList();
        }
    }
}
