using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioEquipo
    {
        void Add(Equipo e);
        void Update(Equipo e);
        void Delete(int id);
        bool EstaEnPrestamo(int equipoId);
        Equipo FindById(int id);
        List<Telescopio> GetTelescopios();
        List<Montura> GetMonturas();
        List<Camara> GetCamaras();
        List<Ocular> GetOculares();
    }
}
