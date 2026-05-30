using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Excepciones;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EquipoController : ControllerBase
    {
        private readonly ICUAltaTelescopio _cuTelescopio;
        private readonly ICUAltaMontura _cuMontura;
        private readonly ICUAltaCamara _cuCamara;
        private readonly ICUAltaOcular _cuOcular;
        private readonly ICUObtenerEquipos _cuObtenerEquipos;
        private readonly ICUBajaEquipo _cuBajaEquipo;
        private readonly ICUEditarTelescopio _cuEditarTelescopio;
        private readonly ICUEditarMontura _cuEditarMontura;
        private readonly ICUEditarCamara _cuEditarCamara;
        private readonly ICUEditarOcular _cuEditarOcular;

        public EquipoController(
            ICUAltaTelescopio cuTelescopio, ICUAltaMontura cuMontura,
            ICUAltaCamara cuCamara, ICUAltaOcular cuOcular,
            ICUObtenerEquipos cuObtenerEquipos, ICUBajaEquipo cuBajaEquipo,
            ICUEditarTelescopio cuEditarTelescopio, ICUEditarMontura cuEditarMontura,
            ICUEditarCamara cuEditarCamara, ICUEditarOcular cuEditarOcular)
        {
            _cuTelescopio = cuTelescopio;
            _cuMontura = cuMontura;
            _cuCamara = cuCamara;
            _cuOcular = cuOcular;
            _cuObtenerEquipos = cuObtenerEquipos;
            _cuBajaEquipo = cuBajaEquipo;
            _cuEditarTelescopio = cuEditarTelescopio;
            _cuEditarMontura = cuEditarMontura;
            _cuEditarCamara = cuEditarCamara;
            _cuEditarOcular = cuEditarOcular;
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            DTOIndexEquipos equipos = _cuObtenerEquipos.ObtenerTodos();
            return Ok(equipos);
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerPorId(int id)
        {
            Equipo equipo = _cuObtenerEquipos.ObtenerPorId(id);
            if (equipo == null)
                return NotFound(new { Message = "Equipo no encontrado." });
            return Ok(equipo);
        }

        [HttpPost("telescopio")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult AltaTelescopio([FromBody] DTOAltaTelescopio dto) => Alta(() => _cuTelescopio.Alta(dto));

        [HttpPost("montura")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult AltaMontura([FromBody] DTOAltaMontura dto) => Alta(() => _cuMontura.Alta(dto));

        [HttpPost("camara")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult AltaCamara([FromBody] DTOAltaCamara dto) => Alta(() => _cuCamara.Alta(dto));

        [HttpPost("ocular")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult AltaOcular([FromBody] DTOAltaOcular dto) => Alta(() => _cuOcular.Alta(dto));

        [HttpPut("telescopio")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult EditarTelescopio([FromBody] DTOAltaTelescopio dto) => Editar(() => _cuEditarTelescopio.Editar(dto));

        [HttpPut("montura")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult EditarMontura([FromBody] DTOAltaMontura dto) => Editar(() => _cuEditarMontura.Editar(dto));

        [HttpPut("camara")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult EditarCamara([FromBody] DTOAltaCamara dto) => Editar(() => _cuEditarCamara.Editar(dto));

        [HttpPut("ocular")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult EditarOcular([FromBody] DTOAltaOcular dto) => Editar(() => _cuEditarOcular.Editar(dto));

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult Baja(int id)
        {
            try
            {
                _cuBajaEquipo.Baja(id);
                return NoContent();
            }
            catch (EquipoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al eliminar el equipo." });
            }
        }

        private IActionResult Alta(Action accion)
        {
            try
            {
                accion();
                return Created("", null);
            }
            catch (EquipoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al dar de alta el equipo." });
            }
        }

        private IActionResult Editar(Action accion)
        {
            try
            {
                accion();
                return NoContent();
            }
            catch (EquipoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al editar el equipo." });
            }
        }
    }
}
