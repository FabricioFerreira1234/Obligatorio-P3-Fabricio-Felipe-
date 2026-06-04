using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
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

        public List<Prestamo> ObtenerPorSocioYEstado(int usuarioId, EstadoPrestamo estado)
        {
            return _context.Prestamos
                .Include(p => p.Telescopio)
                .Include(p => p.Montura)
                .Include(p => p.Visual)
                .Where(p => p.UsuarioId == usuarioId && p.Estado == estado)
                .OrderByDescending(p => p.Fecha.Inicio)
                .ToList();
        }

        // RF09 - Socios (sin repetir) que solicitaron un telescopio dado, ordenados por nombre descendente.
        public List<Usuario> ObtenerSociosPorTelescopio(int telescopioId)
        {
            return _context.Prestamos
                .Where(p => p.TelescopioId == telescopioId)
                .Select(p => p.Usuario)
                .Distinct()
                .OrderByDescending(u => u.NombreCompleto.Nombre)
                .ThenByDescending(u => u.NombreCompleto.Apellido)
                .ToList();
        }

        public Prestamo ObtenerPorId(int id)
        {
            return _context.Prestamos
                .Include(p => p.Telescopio)
                .Include(p => p.Montura)
                .Include(p => p.Visual)
                .FirstOrDefault(p => p.Id == id);
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
