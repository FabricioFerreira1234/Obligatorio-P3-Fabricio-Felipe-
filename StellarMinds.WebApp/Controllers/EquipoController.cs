using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.WebApp.Controllers
{
    [Authorize]
    public class EquipoController : Controller
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

        // INDEX
        public IActionResult Index()
        {
            if (!EsAdministrador()) return Forbid();
            return View(_cuObtenerEquipos.ObtenerTodos());
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuBajaEquipo.Baja(id); }
            catch (EquipoException ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Index");
        }

        // EDIT GET
        public IActionResult Edit(int id)
        {
            if (!EsAdministrador()) return Forbid();
            var equipo = _cuObtenerEquipos.ObtenerPorId(id);
            if (equipo == null) return NotFound();

            return equipo switch
            {
                Telescopio t => View("EditTelescopio", MapperEquipo.ToDTO(t)),
                Montura m => View("EditMontura", MapperEquipo.ToDTO(m)),
                Camara c => View("EditCamara", MapperEquipo.ToDTO(c)),
                Ocular o => View("EditOcular", MapperEquipo.ToDTO(o)),
                _ => NotFound()
            };
        }

        // EDIT POST
        [HttpPost]
        public IActionResult EditTelescopio(DTOAltaTelescopio dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuEditarTelescopio.Editar(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(dto); }
        }

        [HttpPost]
        public IActionResult EditMontura(DTOAltaMontura dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuEditarMontura.Editar(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(dto); }
        }

        [HttpPost]
        public IActionResult EditCamara(DTOAltaCamara dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuEditarCamara.Editar(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(dto); }
        }

        [HttpPost]
        public IActionResult EditOcular(DTOAltaOcular dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuEditarOcular.Editar(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(dto); }
        }

        // CREATE
        public IActionResult Create() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateTelescopio() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateMontura() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateCamara() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateOcular() { if (!EsAdministrador()) return Forbid(); return View(); }

        [HttpPost]
        public IActionResult CreateTelescopio(DTOAltaTelescopio dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuTelescopio.Alta(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(); }
        }

        [HttpPost]
        public IActionResult CreateMontura(DTOAltaMontura dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuMontura.Alta(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(); }
        }

        [HttpPost]
        public IActionResult CreateCamara(DTOAltaCamara dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuCamara.Alta(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(); }
        }

        [HttpPost]
        public IActionResult CreateOcular(DTOAltaOcular dto)
        {
            if (!EsAdministrador()) return Forbid();
            try { _cuOcular.Alta(dto); return RedirectToAction("Index"); }
            catch (Exception ex) { ViewBag.Error = ex.Message; return View(); }
        }

        private bool EsAdministrador() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Administrador;
    }
}