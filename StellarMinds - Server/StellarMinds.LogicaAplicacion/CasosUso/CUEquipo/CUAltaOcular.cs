using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUEquipo
{
    public class CUAltaOcular : ICUAltaOcular
    {
        private  IRepositorioEquipo _repo;
        public CUAltaOcular(IRepositorioEquipo repo) => _repo = repo;
        public void Alta(DTOAltaOcular dto) { var o = MapperEquipo.ToOcular(dto); o.Validar(); _repo.Add(o); }
    }
}
