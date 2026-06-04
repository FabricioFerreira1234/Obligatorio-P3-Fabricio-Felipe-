using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.ValueObjects.VOPrestamo;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.Mappers
{
    public static class MapperPrestamo
    {
       public static Prestamo ToPrestamo(DTOAltaPrestamo dto)
       {
            return new Prestamo {
                Id = dto.Id,
                Fecha = dto.Fecha,
                TelescopioId = dto.TelescopioId,
                MonturaId = dto.MonturaId,
                VisualId = dto.VisualId == 0 ? null : dto.VisualId,
                UsuarioId = dto.UsuarioId,
                Estado = (LogicaNegocio.Enumeraciones.EstadoPrestamo)dto.Estado
            };
        }

        public static DTOPrestamoListado ToDTOListado(Prestamo p)
        {
            return new DTOPrestamoListado
            {
                Id = p.Id,
                FechaInicio = p.Fecha.Inicio,
                FechaFin = p.Fecha.Fin,
                Estado = p.Estado.ToString(),
                Telescopio = p.Telescopio == null ? null : $"{p.Telescopio.Marca} {p.Telescopio.Modelo}",
                Montura = p.Montura == null ? null : $"{p.Montura.Marca} {p.Montura.Modelo}",
                Visual = p.Visual == null ? null : $"{p.Visual.Marca} {p.Visual.Modelo}",
                Atrasado = p.EstaAtrasado()
            };
        }
}
}
