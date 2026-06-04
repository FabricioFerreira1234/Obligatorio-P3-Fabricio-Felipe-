using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.IRepositorios
{
    public interface IRepositorioPrestamo : IRepositorio<Prestamo>
    {
        List<Prestamo> ObtenerPorSocioYMes(int usuarioId, int mes, int anio);
        List<Prestamo> ObtenerPorSocioYEstado(int usuarioId, EstadoPrestamo estado);
        // RF09 - Socios (sin repetir) que solicitaron un telescopio dado, ordenados por nombre descendente.
        List<Usuario> ObtenerSociosPorTelescopio(int telescopioId);
        Prestamo ObtenerPorId(int id);
    }
}
