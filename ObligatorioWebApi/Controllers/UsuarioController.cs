using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Excepciones;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : ControllerBase
    {
        private readonly ICUAltaUsuario _cuAltaUsuario;
        private readonly ICUObtenerUsuarios _cuObtenerUsuarios;
        private readonly ICUEliminar _cuEliminarUsuario;

        public UsuarioController(ICUAltaUsuario cuAltaUsuario,
                                 ICUObtenerUsuarios cuObtenerUsuarios,
                                 ICUEliminar cuEliminarUsuario)
        {
            _cuAltaUsuario = cuAltaUsuario;
            _cuObtenerUsuarios = cuObtenerUsuarios;
            _cuEliminarUsuario = cuEliminarUsuario;
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            try
            {
                List<DTOUsuario> usuarios = _cuObtenerUsuarios.ObtenerUsuarios();
                return Ok(usuarios);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los usuarios." });
            }
        }

        [HttpPost]
        public IActionResult Alta([FromBody] DTOAltaUsuario dto)
        {
            try
            {
                _cuAltaUsuario.AltaUsuario(dto);
                return Created("", dto);
            }
            catch (UsuarioException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al dar de alta el usuario." });
            }
        }

        [HttpDelete("{email}")]
        public IActionResult Eliminar(string email)
        {
            try
            {
                _cuEliminarUsuario.Eliminar(email);
                return NoContent();
            }
            catch (UsuarioException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al eliminar el usuario." });
            }
        }
    }
}
