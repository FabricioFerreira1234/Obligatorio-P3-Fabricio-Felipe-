using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioEquipo : IRepositorioEquipo
    {
        private readonly ApplicationDbContext _context;

        public RepositorioEquipo(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Update(Equipo e)
        {
            _context.Equipos.Update(e);
            _context.SaveChanges();
        }

        public bool EstaEnPrestamo(int equipoId)
        {
            return _context.Prestamos.Any(p =>
                p.Estado == EstadoPrestamo.EN_PRESTAMO &&
                (p.TelescopioId == equipoId ||
                 p.MonturaId == equipoId ||
                 p.VisualId == equipoId));
        }

        public void Add(Equipo e)
        {
            _context.Equipos.Add(e);
            _context.SaveChanges();
        }

        public Equipo FindById(int id) =>
        _context.Equipos.Find(id);

        public void Delete(int id)
        {
            var equipo = _context.Equipos.Find(id);
            if (equipo != null)
            {
                _context.Equipos.Remove(equipo);
                _context.SaveChanges();
            }
        }
        public List<Telescopio> GetTelescopios() =>
            _context.Equipos.OfType<Telescopio>().OrderBy(t => t.Marca).ToList();

        public List<Montura> GetMonturas() =>
            _context.Equipos.OfType<Montura>().OrderBy(m => m.Marca).ToList();

        public List<Camara> GetCamaras() =>
            _context.Equipos.OfType<Camara>().OrderBy(c => c.Marca).ToList();

        public List<Ocular> GetOculares() =>
            _context.Equipos.OfType<Ocular>().OrderBy(o => o.Marca).ToList();
    }
}
