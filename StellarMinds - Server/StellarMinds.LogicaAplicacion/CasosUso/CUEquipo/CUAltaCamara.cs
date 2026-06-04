using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUEquipo
{
    public class CUAltaCamara : ICUAltaCamara
    {
        private  IRepositorioEquipo _repo;
        public CUAltaCamara(IRepositorioEquipo repo) => _repo = repo;
        public void Alta(DTOAltaCamara dto) { var c = MapperEquipo.ToCamara(dto); c.Validar(); _repo.Add(c); }
    }
}
