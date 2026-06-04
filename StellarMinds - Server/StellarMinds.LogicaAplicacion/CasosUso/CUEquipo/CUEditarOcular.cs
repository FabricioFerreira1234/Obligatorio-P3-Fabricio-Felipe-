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
    public class CUEditarOcular : ICUEditarOcular
    {
        private readonly IRepositorioEquipo _repo;
        public CUEditarOcular(IRepositorioEquipo repo) => _repo = repo;
        public void Editar(DTOAltaOcular dto)
        {
            try { var o = MapperEquipo.ToOcular(dto); o.Validar(); _repo.Update(o); }
            catch (Exception ex) { throw new EquipoException(ex.Message); }
        }
    }
}
