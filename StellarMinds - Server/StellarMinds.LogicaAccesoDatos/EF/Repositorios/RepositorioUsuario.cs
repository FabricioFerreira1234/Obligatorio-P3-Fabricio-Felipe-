using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioUsuario : IRepositorioUsuario
    {
        private  ApplicationDbContext _context;

        public RepositorioUsuario(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Usuario u)
        {
            _context.Usuarios.Add(u);
            _context.SaveChanges();
        }

        public List<Usuario> FindAll()
        {
            return _context.Usuarios
                .OrderBy(u => u.NombreCompleto.Apellido)
                .ThenBy(u => u.Email)
                .ToList();
        }

        public Usuario FindByEmail(string email)
        {
            return _context.Usuarios
                .SingleOrDefault(u => u.Email == email);
           
        }

        public void Delete(Usuario u)
        {
            _context.Usuarios.Remove(u);
            _context.SaveChanges();
        }
    }
}