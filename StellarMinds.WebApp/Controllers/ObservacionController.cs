using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Enums;
using StellarMinds.WebApp.Models;
using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Controllers
{
    // RF07 - Alta de observación (Rol Socio). Consume la WebAPI vía ClienteHttpAuxiliar (token Bearer).
    [Authorize]
    public class ObservacionController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            if (!EsSocio()) return Forbid();

            var modelo = new ObservacionCreateViewModel { Fecha = DateTime.Today };
            CargarListas(modelo);
            return View(modelo);
        }

        // Botón "Ver evaluación": evalúa la adecuación sin guardar y vuelve a mostrar el formulario.
        [HttpPost]
        public IActionResult Evaluar(ObservacionCreateViewModel modelo)
        {
            if (!EsSocio()) return Forbid();

            string token = HttpContext.Session.GetString("token");
            var dto = new AltaObservacionModel
            {
                PrestamoId = modelo.PrestamoId,
                ObjetoCelesteId = modelo.ObjetoCelesteId,
                Fecha = modelo.Fecha
            };

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Observacion/evaluar", VerbosHttp.POST, dto, token);

            if (respuesta.IsSuccessStatusCode)
            {
                ResultadoEvaluacionModel resultado = JsonConvert.DeserializeObject<ResultadoEvaluacionModel>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta));
                modelo.Evaluado = true;
                modelo.Indicador = resultado?.Indicador;
                modelo.Detalle = resultado?.Detalle;
            }
            else
            {
                ViewBag.Error = RespuestaApi.LeerError(respuesta);
            }

            CargarListas(modelo);
            return View("Create", modelo);
        }

        // Botón "Confirmar alta": registra la observación con el resultado de la evaluación.
        [HttpPost]
        public IActionResult Confirmar(ObservacionCreateViewModel modelo)
        {
            if (!EsSocio()) return Forbid();

            string token = HttpContext.Session.GetString("token");
            var dto = new AltaObservacionModel
            {
                PrestamoId = modelo.PrestamoId,
                ObjetoCelesteId = modelo.ObjetoCelesteId,
                Fecha = modelo.Fecha
            };

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Observacion", VerbosHttp.POST, dto, token);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Observación registrada correctamente.";
                return RedirectToAction("Create");
            }

            ViewBag.Error = RespuestaApi.LeerError(respuesta);
            CargarListas(modelo);
            return View("Create", modelo);
        }

        private void CargarListas(ObservacionCreateViewModel modelo)
        {
            string token = HttpContext.Session.GetString("token");

            HttpResponseMessage rPrestamos = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Observacion/prestamos-vigentes", VerbosHttp.GET, null, token);
            if (rPrestamos.IsSuccessStatusCode)
                modelo.Prestamos = JsonConvert.DeserializeObject<List<PrestamoListadoModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(rPrestamos)) ?? new();

            HttpResponseMessage rObjetos = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Observacion/objetos", VerbosHttp.GET, null, token);
            if (rObjetos.IsSuccessStatusCode)
                modelo.Objetos = JsonConvert.DeserializeObject<List<ObjetoCelesteModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(rObjetos)) ?? new();
        }

        private bool EsSocio() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Socio;
    }
}
