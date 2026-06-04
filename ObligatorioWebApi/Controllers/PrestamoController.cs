using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using System.Security.Claims;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrestamoController : ControllerBase
    {
        private readonly ICUListadoPrestamosSocio _cuListado;
        private readonly ICUAltaPrestamo _cuAlta;
        private readonly ICUObtenerUsuarios _cuObtenerUsuarios;
        private readonly ICUListadoPrestamosEnPrestamoSocio _cuListadoEnPrestamo;
        private readonly ICUDevolverPrestamo _cuDevolver;
        private readonly ICUSociosPorTelescopio _cuSociosPorTelescopio;

        public PrestamoController(ICUListadoPrestamosSocio cuListado, ICUAltaPrestamo cuAlta, ICUObtenerUsuarios cuObtenerUsuarios,
            ICUListadoPrestamosEnPrestamoSocio cuListadoEnPrestamo, ICUDevolverPrestamo cuDevolver,
            ICUSociosPorTelescopio cuSociosPorTelescopio)
        {
            _cuListado = cuListado;
            _cuAlta = cuAlta;
            _cuObtenerUsuarios = cuObtenerUsuarios;
            _cuListadoEnPrestamo = cuListadoEnPrestamo;
            _cuDevolver = cuDevolver;
            _cuSociosPorTelescopio = cuSociosPorTelescopio;
        }

        // RF08 - Listado de préstamos del socio logueado en un mes y año dados.
        [HttpGet]
        [Authorize(Roles = "Socio")]
        public IActionResult ObtenerPorMes([FromQuery] int mes, [FromQuery] int anio)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                List<DTOPrestamoListado> prestamos = _cuListado.Ejecutar(email, mes, anio);

                // Se devuelve siempre la lista (vacía si no hay): el mensaje descriptivo del
                // "no hay préstamos" lo muestra la WebApp. Así el cliente tipado no rompe al
                // deserializar (un objeto { Message } no encaja en List<DTOPrestamoListado>).
                return Ok(prestamos);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los préstamos." });
            }
        }

        // Alta de préstamo hecha por el propio socio.
        [HttpPost]
        [Authorize(Roles = "Socio")]
        public IActionResult Alta([FromBody] DTOAltaPrestamo dto)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                _cuAlta.Ejecutar(dto, email);
                return Created("", null);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al registrar el préstamo." });
            }
        }

        // RF04 - Socios disponibles para que el Coordinador elija a quién prestar.
        [HttpGet("socios")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult ObtenerSocios()
        {
            try
            {
                List<DTOUsuario> socios = _cuObtenerUsuarios.ObtenerUsuarios()
                    .Where(u => u.TipoUsuario == TipoUsuario.Socio)
                    .ToList();
                return Ok(socios);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los socios." });
            }
        }

        // RF04 - Alta de préstamo hecha por el Coordinador para un socio dado.
        [HttpPost("coordinador")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult AltaPorCoordinador([FromBody] DTOAltaPrestamo dto)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                _cuAlta.EjecutarComoCoordinador(dto, email);
                return Created("", null);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al registrar el préstamo." });
            }
        }

        // RF05 - Préstamos EN PRÉSTAMO de un socio, para que el Coordinador elija cuál devolver.
        [HttpGet("socio/{socioId}/activos")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult ObtenerActivosPorSocio(int socioId)
        {
            try
            {
                List<DTOPrestamoListado> prestamos = _cuListadoEnPrestamo.Ejecutar(socioId);
                return Ok(prestamos);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los préstamos del socio." });
            }
        }

        // RF05 - Devolución de un préstamo: pasa a DEVUELTO y reintegra la cantidad de los equipos.
        [HttpPut("{id}/devolver")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult Devolver(int id)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                _cuDevolver.Ejecutar(id, email);
                return Ok(new { Message = "Devolución registrada correctamente." });
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al registrar la devolución." });
            }
        }

        // RF09 - Socios (sin repetir) a los que se les prestó un telescopio dado, ordenados por nombre desc.
        [HttpGet("telescopio/{telescopioId}/socios")]
        [Authorize(Roles = "Administrador,Coordinador")]
        public IActionResult SociosPorTelescopio(int telescopioId)
        {
            try
            {
                List<DTOSocioTelescopio> socios = _cuSociosPorTelescopio.Ejecutar(telescopioId);
                return Ok(socios);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los socios del telescopio." });
            }
        }
    }
}
