using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUEquipo
{
    public class CUAltaMontura : ICUAltaMontura
    {
        private  IRepositorioEquipo _repo;
        public CUAltaMontura(IRepositorioEquipo repo) => _repo = repo;
        public void Alta(DTOAltaMontura dto) { var m = MapperEquipo.ToMontura(dto); m.Validar(); _repo.Add(m); }
    }
}
