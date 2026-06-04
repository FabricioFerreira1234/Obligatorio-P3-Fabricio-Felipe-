using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioObjetoCeleste : IRepositorioObjetoCeleste
    {
        private ApplicationDbContext _context;

        public RepositorioObjetoCeleste(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ObjetoCeleste> FindAll()
        {
            return _context.ObjetosCelestes
                .OrderBy(o => o.Nombre)
                .ToList();
        }

        public ObjetoCeleste ObtenerPorId(int id)
        {
            return _context.ObjetosCelestes.FirstOrDefault(o => o.Id == id);
        }

        public void Add(ObjetoCeleste entity)
        {
            _context.ObjetosCelestes.Add(entity);
            _context.SaveChanges();
        }

        public void Update(ObjetoCeleste entity)
        {
            _context.ObjetosCelestes.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(ObjetoCeleste entity)
        {
            throw new NotImplementedException();
        }

        public ObjetoCeleste FindById(object id)
        {
            return _context.ObjetosCelestes.Find(id);
        }
    }
}
