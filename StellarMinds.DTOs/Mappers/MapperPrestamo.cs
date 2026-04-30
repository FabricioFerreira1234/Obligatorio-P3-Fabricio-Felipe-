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
                VisualId = dto.VisualId,
                Estado = (LogicaNegocio.Enumeraciones.EstadoPrestamo)dto.Estado
            };
        }

}
}
