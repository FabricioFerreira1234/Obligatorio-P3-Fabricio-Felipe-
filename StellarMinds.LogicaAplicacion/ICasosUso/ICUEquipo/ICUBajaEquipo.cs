using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo
{
    // ICUBajaEquipo.cs
    public interface ICUBajaEquipo
    {
        void Baja(int id);
    }

    // CUBajaEquipo.cs
    public class CUBajaEquipo : ICUBajaEquipo
    {
        private readonly IRepositorioEquipo _repo;
        public CUBajaEquipo(IRepositorioEquipo repo) => _repo = repo;

        public void Baja(int id)
        {
            // TODO: validar que el equipo no esté en préstamo activo
            var equipo = _repo.FindById(id);
            if (equipo == null) throw new EquipoException("Equipo no encontrado.");
            _repo.Delete(id);
        }
    }
}
