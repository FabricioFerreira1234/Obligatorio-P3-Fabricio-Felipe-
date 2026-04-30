using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo
{
    public class CUAltaPrestamo : ICUAltaPrestamo
    { 
         private IRepositorioPrestamo _repoPrestamo;
        public CUAltaPrestamo(IRepositorioPrestamo repoPrestamo)=> _repoPrestamo = repoPrestamo;

        public void Ejecutar(DTOAltaPrestamo p)
        {
            Prestamo Nuevo = MapperPrestamo.ToPrestamo(p);
            Nuevo.Validar();
             _repoPrestamo.Add(Nuevo);
        }

    }
}
