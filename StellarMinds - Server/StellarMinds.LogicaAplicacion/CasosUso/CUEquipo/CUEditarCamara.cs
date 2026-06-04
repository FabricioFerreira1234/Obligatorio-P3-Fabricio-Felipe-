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
    public class CUEditarCamara : ICUEditarCamara
    {
        private readonly IRepositorioEquipo _repo;
        public CUEditarCamara(IRepositorioEquipo repo) => _repo = repo;
        public void Editar(DTOAltaCamara dto)
        {
            try { var c = MapperEquipo.ToCamara(dto); c.Validar(); _repo.Update(c); }
            catch (Exception ex) { throw new EquipoException(ex.Message); }
        }
    }
}
