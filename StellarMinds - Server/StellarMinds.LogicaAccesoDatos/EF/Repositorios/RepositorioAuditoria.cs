using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioAuditoria : IRepositorioAuditoria
    {
        private ApplicationDbContext _context;

        public RepositorioAuditoria(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Auditoria a)
        {
            _context.Auditorias.Add(a);
            _context.SaveChanges();
        }

        public List<Auditoria> FindAll()
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Prestamo)
                .OrderByDescending(a => a.FechaHora)
                .ToList();
        }

        // RF11 - Altas (acción PRESTAMO) realizadas por un coordinador (o todas si no se filtra).
        public List<Auditoria> ObtenerAltas(int? coordinadorId)
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Include(a => a.Prestamo).ThenInclude(p => p.Telescopio)
                .Include(a => a.Prestamo).ThenInclude(p => p.Montura)
                .Include(a => a.Prestamo).ThenInclude(p => p.Visual)
                .Include(a => a.Prestamo).ThenInclude(p => p.Usuario)
                .Where(a => a.Accion == TipoAccionAuditoria.PRESTAMO
                            && (!coordinadorId.HasValue || a.UsuarioId == coordinadorId.Value))
                .OrderByDescending(a => a.FechaHora)
                .ToList();
        }

        // RF11 - Acciones auditadas (préstamo y devolución) de un préstamo dado.
        public List<Auditoria> ObtenerPorPrestamo(int prestamoId)
        {
            return _context.Auditorias
                .Include(a => a.Usuario)
                .Where(a => a.PrestamoId == prestamoId)
                .OrderBy(a => a.FechaHora)
                .ToList();
        }
    }
}
