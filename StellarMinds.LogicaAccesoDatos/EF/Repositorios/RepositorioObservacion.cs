using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StellarMinds.LogicaAccesoDatos.EF.Repositorios
{
    public class RepositorioObservacion : IRepositorioObservacion
    {
        private ApplicationDbContext _context;

        public RepositorioObservacion(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Observacion entity)
        {
            _context.Observaciones.Add(entity);
            _context.SaveChanges();
        }

        public List<Observacion> FindAll()
        {
            return _context.Observaciones
                .Include(o => o.Prestamo)
                .Include(o => o.ObjetoCeleste)
                .OrderByDescending(o => o.Fecha)
                .ToList();
        }

        // RF10 - Ranking de objetos celestes observados: agrupa las observaciones por objeto,
        // cuenta cuántas veces fue observado cada uno y ordena de mayor a menor. Al partir de
        // la tabla de observaciones, los objetos nunca observados quedan naturalmente fuera.
        public List<(ObjetoCeleste Objeto, int Cantidad)> ObtenerRankingObservados()
        {
            return _context.Observaciones
                .GroupBy(o => o.ObjetoCeleste)
                .Select(g => new { Objeto = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad)
                .ToList()
                .Select(x => (x.Objeto, x.Cantidad))
                .ToList();
        }

        public void Update(Observacion entity)
        {
            _context.Observaciones.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(Observacion entity)
        {
            throw new NotImplementedException();
        }

        public Observacion FindById(object id)
        {
            return _context.Observaciones.Find(id);
        }
    }
}
