using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;

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
    }
}
