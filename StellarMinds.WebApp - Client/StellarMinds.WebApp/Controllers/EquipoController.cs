using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Enums;
using StellarMinds.WebApp.Filter;
using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Controllers
{
    // Gestión de equipos (Rol Administrador). Consume la WebAPI vía ClienteHttpAuxiliar (token Bearer).
    [LoginFilter]
    public class EquipoController : Controller
    {
        private string Token => HttpContext.Session.GetString("token");

        // INDEX
        public IActionResult Index()
        {
            if (!EsAdministrador()) return Forbid();
            IndexEquiposModel equipos = ObtenerEquipos();
            return View(equipos);
        }

        // DELETE
        public IActionResult Delete(int id)
        {
            if (!EsAdministrador()) return Forbid();
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Equipo/" + id, VerbosHttp.DELETE, null, Token);
            if (!respuesta.IsSuccessStatusCode)
                TempData["Error"] = RespuestaApi.LeerError(respuesta);
            return RedirectToAction("Index");
        }

        // EDIT GET: ubica el equipo dentro del listado para saber su tipo y precargar el formulario.
        public IActionResult Edit(int id)
        {
            if (!EsAdministrador()) return Forbid();
            IndexEquiposModel equipos = ObtenerEquipos();

            var t = equipos.Telescopios.FirstOrDefault(x => x.Id == id);
            if (t != null)
                return View("EditTelescopio", new AltaTelescopioModel
                {
                    Id = t.Id, Marca = t.Marca, Modelo = t.Modelo, Cantidad = t.Cantidad,
                    Apertura = t.Apertura, RelacionFocal = t.RelacionFocal, DistanciaFocal = t.DistanciaFocal, Peso = t.Peso
                });

            var m = equipos.Monturas.FirstOrDefault(x => x.Id == id);
            if (m != null)
                return View("EditMontura", new AltaMonturaModel
                {
                    Id = m.Id, Marca = m.Marca, Modelo = m.Modelo, Cantidad = m.Cantidad,
                    Tipo = m.Tipo, PesoMaximo = m.PesoMaximo, Goto = m.Goto
                });

            var c = equipos.Camaras.FirstOrDefault(x => x.Id == id);
            if (c != null)
                return View("EditCamara", new AltaCamaraModel
                {
                    Id = c.Id, Marca = c.Marca, Modelo = c.Modelo, Cantidad = c.Cantidad,
                    Sensor = c.Sensor, Resolucion = c.Resolucion, PixelSize = c.PixelSize
                });

            var o = equipos.Oculares.FirstOrDefault(x => x.Id == id);
            if (o != null)
                return View("EditOcular", new AltaOcularModel
                {
                    Id = o.Id, Marca = o.Marca, Modelo = o.Modelo, Cantidad = o.Cantidad,
                    Diametro = o.Diametro, AnguloVisual = o.AnguloVisual
                });

            return NotFound();
        }

        // EDIT POST
        [HttpPost]
        public IActionResult EditTelescopio(AltaTelescopioModel dto) => Enviar(VerbosHttp.PUT, "Equipo/telescopio", dto, dto);

        [HttpPost]
        public IActionResult EditMontura(AltaMonturaModel dto) => Enviar(VerbosHttp.PUT, "Equipo/montura", dto, dto);

        [HttpPost]
        public IActionResult EditCamara(AltaCamaraModel dto) => Enviar(VerbosHttp.PUT, "Equipo/camara", dto, dto);

        [HttpPost]
        public IActionResult EditOcular(AltaOcularModel dto) => Enviar(VerbosHttp.PUT, "Equipo/ocular", dto, dto);

        // CREATE
        public IActionResult Create() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateTelescopio() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateMontura() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateCamara() { if (!EsAdministrador()) return Forbid(); return View(); }
        public IActionResult CreateOcular() { if (!EsAdministrador()) return Forbid(); return View(); }

        [HttpPost]
        public IActionResult CreateTelescopio(AltaTelescopioModel dto) => Enviar(VerbosHttp.POST, "Equipo/telescopio", dto, null);

        [HttpPost]
        public IActionResult CreateMontura(AltaMonturaModel dto) => Enviar(VerbosHttp.POST, "Equipo/montura", dto, null);

        [HttpPost]
        public IActionResult CreateCamara(AltaCamaraModel dto) => Enviar(VerbosHttp.POST, "Equipo/camara", dto, null);

        [HttpPost]
        public IActionResult CreateOcular(AltaOcularModel dto) => Enviar(VerbosHttp.POST, "Equipo/ocular", dto, null);

        // Envía el alta/edición de un equipo; si falla, vuelve a la vista mostrando el error.
        // vistaModelo: modelo a devolver en caso de error (en edición se reusa el dto; en alta queda null).
        private IActionResult Enviar(VerbosHttp verbo, string recurso, object dto, object vistaModelo)
        {
            if (!EsAdministrador()) return Forbid();
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + recurso, verbo, dto, Token);
            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction("Index");
            ViewBag.Error = RespuestaApi.LeerError(respuesta);
            return vistaModelo != null ? View(vistaModelo) : View();
        }

        private IndexEquiposModel ObtenerEquipos()
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Equipo", VerbosHttp.GET, null, Token);
            if (respuesta.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<IndexEquiposModel>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            return new IndexEquiposModel();
        }

        private bool EsAdministrador() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Administrador;
    }
}
