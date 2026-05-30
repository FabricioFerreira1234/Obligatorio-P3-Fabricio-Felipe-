using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaNegocio.Excepciones;
using System.Security.Claims;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Socio")]
    public class PrestamoController : ControllerBase
    {
        private readonly ICUListadoPrestamosSocio _cuListado;
        private readonly ICUAltaPrestamo _cuAlta;

        public PrestamoController(ICUListadoPrestamosSocio cuListado, ICUAltaPrestamo cuAlta)
        {
            _cuListado = cuListado;
            _cuAlta = cuAlta;
        }

        // RF08 - Listado de préstamos del socio logueado en un mes y año dados.
        [HttpGet]
        public IActionResult ObtenerPorMes([FromQuery] int mes, [FromQuery] int anio)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                List<DTOPrestamoListado> prestamos = _cuListado.Ejecutar(email, mes, anio);

                if (prestamos.Count == 0)
                    return Ok(new { Message = $"No existen préstamos para el mes {mes}/{anio}." });

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

        [HttpPost]
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
    }
}
