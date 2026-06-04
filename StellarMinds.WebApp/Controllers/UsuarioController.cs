using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Controllers
{
    // Gestión de usuarios (Rol Administrador). Consume la WebAPI vía ClienteHttpAuxiliar (token Bearer).
    [Authorize]
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            if (!EsAdministrador()) return Forbid();

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Usuario", VerbosHttp.GET, null, token);

            if (respuesta.IsSuccessStatusCode)
            {
                var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
                return View(usuarios);
            }

            ViewBag.Error = RespuestaApi.LeerError(respuesta);
            return View(new List<UsuarioModel>());
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!EsAdministrador()) return Forbid();
            return View(new AltaUsuarioModel());
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AltaUsuarioModel u)
        {
            if (!EsAdministrador()) return Forbid();
            if (!ModelState.IsValid) return View(u);

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Usuario", VerbosHttp.POST, u, token);

            if (respuesta.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Error = RespuestaApi.LeerError(respuesta);
            return View(u);
        }

        public IActionResult Delete(string email)
        {
            if (!EsAdministrador()) return Forbid();

            string token = HttpContext.Session.GetString("token");
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Usuario/" + email, VerbosHttp.DELETE, null, token);

            if (!respuesta.IsSuccessStatusCode)
                TempData["Error"] = RespuestaApi.LeerError(respuesta);

            return RedirectToAction("Index");
        }

        private bool EsAdministrador()
        {
            var tipo = User.FindFirst("TipoUsuario")?.Value;
            return Enum.TryParse<TipoUsuario>(tipo, out var t) && t == TipoUsuario.Administrador;
        }
    }
}
