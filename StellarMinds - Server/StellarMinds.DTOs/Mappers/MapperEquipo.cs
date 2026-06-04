using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.Mappers
{
    public static class MapperEquipo
    {
        public static DTOAltaTelescopio ToDTO(Telescopio t) => new DTOAltaTelescopio
        {
            Id = t.Id,
            Marca = t.Marca,
            Modelo = t.Modelo,
            Cantidad = t.Cantidad,
            Apertura = t.Apertura,
            RelacionFocal = t.RelacionFocal,
            DistanciaFocal = t.DistanciaFocal,
            Peso = t.Peso
        };

        public static DTOAltaMontura ToDTO(Montura m) => new DTOAltaMontura
        {
            Id = m.Id,
            Marca = m.Marca,
            Modelo = m.Modelo,
            Cantidad = m.Cantidad,
            Tipo = m.Tipo,
            PesoMaximo = m.PesoMaximo,
            Goto = m.Goto
        };

        public static DTOAltaCamara ToDTO(Camara c) => new DTOAltaCamara
        {
            Id = c.Id,
            Marca = c.Marca,
            Modelo = c.Modelo,
            Cantidad = c.Cantidad,
            Sensor = c.Sensor,
            Resolucion = c.Resolucion,
            PixelSize = c.PixelSize
        };

        public static DTOAltaOcular ToDTO(Ocular o) => new DTOAltaOcular
        {
            Id = o.Id,
            Marca = o.Marca,
            Modelo = o.Modelo,
            Cantidad = o.Cantidad,
            Diametro = o.Diametro,
            AnguloVisual = o.AnguloVisual
        };

        // --- DTO → Entidad (los que ya tenías, ahora con Id) ---

        public static Telescopio ToTelescopio(DTOAltaTelescopio dto) => new Telescopio
        {
            Id = dto.Id,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Cantidad = dto.Cantidad,
            Apertura = dto.Apertura,
            RelacionFocal = dto.RelacionFocal,
            DistanciaFocal = dto.DistanciaFocal,
            Peso = dto.Peso
        };

        public static Montura ToMontura(DTOAltaMontura dto) => new Montura
        {
            Id = dto.Id,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Cantidad = dto.Cantidad,
            Tipo = dto.Tipo,
            PesoMaximo = dto.PesoMaximo,
            Goto = dto.Goto
        };

        public static Camara ToCamara(DTOAltaCamara dto) => new Camara
        {
            Id = dto.Id,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Cantidad = dto.Cantidad,
            Sensor = dto.Sensor,
            Resolucion = dto.Resolucion,
            PixelSize = dto.PixelSize
        };

        public static Ocular ToOcular(DTOAltaOcular dto) => new Ocular
        {
            Id = dto.Id,
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Cantidad = dto.Cantidad,
            Diametro = dto.Diametro,
            AnguloVisual = dto.AnguloVisual
        };
    }
}
