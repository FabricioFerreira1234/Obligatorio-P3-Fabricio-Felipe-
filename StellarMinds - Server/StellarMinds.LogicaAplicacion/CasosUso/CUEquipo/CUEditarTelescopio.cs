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
    public class CUEditarTelescopio : ICUEditarTelescopio
    {
        private readonly IRepositorioEquipo _repo;
        public CUEditarTelescopio(IRepositorioEquipo repo) => _repo = repo;
        public void Editar(DTOAltaTelescopio dto)
        {
            try { var t = MapperEquipo.ToTelescopio(dto); t.Validar(); _repo.Update(t); }
            catch (Exception ex) { throw new EquipoException(ex.Message); }
        }
    }
}
