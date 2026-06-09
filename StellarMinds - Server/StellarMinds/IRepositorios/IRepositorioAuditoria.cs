using StellarMinds.LogicaNegocio.Entidades;
using System.Collections.Generic;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioAuditoria
    {
        void Add(Auditoria a);
        List<Auditoria> FindAll();
        // RF11 - Altas (acción PRESTAMO) realizadas por un coordinador (o todas si no se filtra),
        // con el préstamo, sus equipos, el socio y el coordinador cargados.
        List<Auditoria> ObtenerAltas(int? coordinadorId);
        // RF11 - Todas las acciones auditadas (préstamo y devolución) de un préstamo, con el coordinador.
        List<Auditoria> ObtenerPorPrestamo(int prestamoId);
    }
}
