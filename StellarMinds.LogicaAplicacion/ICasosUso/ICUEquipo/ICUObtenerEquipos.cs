using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo
{
    public interface ICUObtenerEquipos
    {
        DTOIndexEquipos ObtenerTodos();
        Equipo ObtenerPorId(int id);
    }

    public class CUObtenerEquipos : ICUObtenerEquipos
    {
        private readonly IRepositorioEquipo _repo;
        public CUObtenerEquipos(IRepositorioEquipo repo) => _repo = repo;

        public DTOIndexEquipos ObtenerTodos() => new DTOIndexEquipos
        {
            Telescopios = _repo.GetTelescopios(),
            Monturas = _repo.GetMonturas(),
            Camaras = _repo.GetCamaras(),
            Oculares = _repo.GetOculares()
        };

        public Equipo ObtenerPorId(int id) => _repo.FindById(id);
    }
}
