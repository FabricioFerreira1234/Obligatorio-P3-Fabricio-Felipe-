using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste;
using StellarMinds.DTOs.DataTransferObjects.DTOsObservacion;
using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaNegocio.Excepciones;
using System.Collections.Generic;
using System.Security.Claims;

namespace ObligatorioWebApi.Controllers
{
    // RF07 - Alta de observación (Rol Socio).
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Socio")]
    public class ObservacionController : ControllerBase
    {
        private readonly ICUObtenerObjetosCelestes _cuObjetos;
        private readonly ICUObtenerPrestamosVigentesSocio _cuPrestamosVigentes;
        private readonly ICUEvaluarAdecuacion _cuEvaluar;
        private readonly ICUAltaObservacion _cuAlta;

        public ObservacionController(ICUObtenerObjetosCelestes cuObjetos,
            ICUObtenerPrestamosVigentesSocio cuPrestamosVigentes,
            ICUEvaluarAdecuacion cuEvaluar, ICUAltaObservacion cuAlta)
        {
            _cuObjetos = cuObjetos;
            _cuPrestamosVigentes = cuPrestamosVigentes;
            _cuEvaluar = cuEvaluar;
            _cuAlta = cuAlta;
        }

        // Listado de objetos celestes preingresados.
        [HttpGet("objetos")]
        public IActionResult ObtenerObjetos()
        {
            try
            {
                List<DTOObjetoCeleste> objetos = _cuObjetos.Ejecutar();
                return Ok(objetos);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los objetos celestes." });
            }
        }

        // Préstamos no devueltos y vigentes del socio logueado.
        [HttpGet("prestamos-vigentes")]
        public IActionResult ObtenerPrestamosVigentes()
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                List<DTOPrestamoListado> prestamos = _cuPrestamosVigentes.Ejecutar(email);
                return Ok(prestamos);
            }
            catch (ObservacionException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los préstamos vigentes." });
            }
        }

        // Evalúa la adecuación del equipo para el objeto celeste SIN guardar (botón "ver evaluación").
        [HttpPost("evaluar")]
        public IActionResult Evaluar([FromBody] DTOAltaObservacion dto)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                DTOResultadoEvaluacion resultado = _cuEvaluar.Ejecutar(email, dto);
                return Ok(resultado);
            }
            catch (ObservacionException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al evaluar la adecuación." });
            }
        }

        // Confirma el alta de la observación, guardando el resultado de la evaluación.
        [HttpPost]
        public IActionResult Alta([FromBody] DTOAltaObservacion dto)
        {
            try
            {
                string email = User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
                _cuAlta.Ejecutar(email, dto);
                return Created("", null);
            }
            catch (ObservacionException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al registrar la observación." });
            }
        }
    }
}
