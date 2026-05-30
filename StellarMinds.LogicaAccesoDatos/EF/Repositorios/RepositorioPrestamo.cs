using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioPrestamo : IRepositorioPrestamo
    {
        private ApplicationDbContext _context;
        public RepositorioPrestamo(ApplicationDbContext context)
        {
            _context = context;
        }

        public Prestamo BuscarPrestamos(int prestamosId) => _context.Prestamos.Find(prestamosId);

        public List<Prestamo> ObtenerPorSocioYMes(int usuarioId, int mes, int anio)
        {
            return _context.Prestamos
                .Include(p => p.Telescopio)
                .Include(p => p.Montura)
                .Include(p => p.Visual)
                .Where(p => p.UsuarioId == usuarioId
                            && p.Fecha.Inicio.Month == mes
                            && p.Fecha.Inicio.Year == anio)
                .OrderBy(p => p.Fecha.Inicio)
                .ToList();
        }

        public void Add(Prestamo p)
        {
            _context.Prestamos.Add(p);
            _context.SaveChanges();
        }
        public void Update(Prestamo p)
        {
            _context.Prestamos.Update(p);
            _context.SaveChanges();
        }


        public void Delete(Prestamo entity)
        {
            throw new NotImplementedException();
        }

        public Prestamo FindById(object id)
        {
            throw new NotImplementedException();
        }
    }
}
