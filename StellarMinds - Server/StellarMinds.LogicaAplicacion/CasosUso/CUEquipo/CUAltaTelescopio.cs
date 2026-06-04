using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUEquipo
{
    public class CUAltaTelescopio : ICUAltaTelescopio
    {
        private  IRepositorioEquipo _repo;
        public CUAltaTelescopio(IRepositorioEquipo repo) => _repo = repo;

        public void Alta(DTOAltaTelescopio dto)
        {
            try
            {
                Telescopio t = MapperEquipo.ToTelescopio(dto);
                t.Validar();
                _repo.Add(t);
            }
            catch (Exception ex) 
            {
                throw new EquipoException(ex.Message);
            }
        }
    }
}
