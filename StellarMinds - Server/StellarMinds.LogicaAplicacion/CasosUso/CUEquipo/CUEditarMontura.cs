using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.CasosUso.CUEquipo
{
    public class CUEditarMontura : ICUEditarMontura
    {
        private readonly IRepositorioEquipo _repo;
        public CUEditarMontura(IRepositorioEquipo repo) => _repo = repo;
        public void Editar(DTOAltaMontura dto)
        {
            try { var m = MapperEquipo.ToMontura(dto); m.Validar(); _repo.Update(m); }
            catch (Exception ex) { throw new EquipoException(ex.Message); }
        }
    }
}
