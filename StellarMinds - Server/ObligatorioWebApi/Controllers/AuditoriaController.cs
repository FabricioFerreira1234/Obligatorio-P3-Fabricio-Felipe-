using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsAuditoria;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObligatorioWebApi.Controllers
{
    // RF11 - Información de auditoría de préstamos (Rol Administrador).
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrador")]
    public class AuditoriaController : ControllerBase
    {
        private readonly ICUListadoPrestamosAuditoria _cuListado;
        private readonly ICUAuditoriaPrestamo _cuAuditoria;
        private readonly ICUDetallePrestamo _cuDetalle;
        private readonly ICUObtenerUsuarios _cuObtenerUsuarios;

        public AuditoriaController(ICUListadoPrestamosAuditoria cuListado, ICUAuditoriaPrestamo cuAuditoria,
            ICUDetallePrestamo cuDetalle, ICUObtenerUsuarios cuObtenerUsuarios)
        {
            _cuListado = cuListado;
            _cuAuditoria = cuAuditoria;
            _cuDetalle = cuDetalle;
            _cuObtenerUsuarios = cuObtenerUsuarios;
        }

        // Listado de préstamos realizados por coordinadores, filtrable por coordinador.
        [HttpGet("prestamos")]
        public IActionResult Prestamos([FromQuery] int? coordinadorId)
        {
            try
            {
                List<DTOPrestamoAuditoria> prestamos = _cuListado.Ejecutar(coordinadorId);
                return Ok(prestamos);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los préstamos auditados." });
            }
        }

        // Coordinadores disponibles para el filtro.
        [HttpGet("coordinadores")]
        public IActionResult Coordinadores()
        {
            try
            {
                List<DTOUsuario> coordinadores = _cuObtenerUsuarios.ObtenerUsuarios()
                    .Where(u => u.TipoUsuario == TipoUsuario.Coordinador)
                    .ToList();
                return Ok(coordinadores);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener los coordinadores." });
            }
        }

        // Acciones auditadas de un préstamo (préstamo y devolución).
        [HttpGet("prestamo/{prestamoId}")]
        public IActionResult AuditoriaPrestamo(int prestamoId)
        {
            try
            {
                List<DTOAuditoriaItem> items = _cuAuditoria.Ejecutar(prestamoId);
                return Ok(items);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener la auditoría del préstamo." });
            }
        }

        // Detalle completo de un préstamo (link de detalles).
        [HttpGet("prestamo/{prestamoId}/detalle")]
        public IActionResult Detalle(int prestamoId)
        {
            try
            {
                DTOPrestamoAuditoria detalle = _cuDetalle.Ejecutar(prestamoId);
                return Ok(detalle);
            }
            catch (PrestamoException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener el detalle del préstamo." });
            }
        }
    }
}
