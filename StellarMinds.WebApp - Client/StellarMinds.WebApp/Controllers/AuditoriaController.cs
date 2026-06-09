using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Enums;
using StellarMinds.WebApp.Filter;
using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Controllers
{
    // RF11 - Información de auditoría de préstamos (Rol Administrador). Consume la WebAPI vía ClienteHttpAuxiliar.
    [LoginFilter]
    public class AuditoriaController : Controller
    {
        private string Token => HttpContext.Session.GetString("token");

        // Listado de préstamos realizados por coordinadores, filtrable por coordinador.
        [HttpGet]
        public IActionResult Index(int? coordinadorId)
        {
            if (!EsAdministrador()) return Forbid();

            var modelo = new AuditoriaIndexViewModel
            {
                CoordinadorId = coordinadorId,
                Coordinadores = ObtenerLista<UsuarioModel>("Auditoria/coordinadores")
            };

            string recurso = coordinadorId.HasValue && coordinadorId.Value > 0
                ? $"Auditoria/prestamos?coordinadorId={coordinadorId.Value}"
                : "Auditoria/prestamos";
            modelo.Prestamos = ObtenerLista<PrestamoAuditoriaModel>(recurso);

            return View(modelo);
        }

        // RF11 - Información de auditoría de un préstamo en una nueva vista (acciones, fecha y coordinador).
        [HttpGet]
        public IActionResult Auditoria(int prestamoId)
        {
            if (!EsAdministrador()) return Forbid();

            var modelo = new AuditoriaPrestamoViewModel
            {
                PrestamoId = prestamoId,
                Items = ObtenerLista<AuditoriaItemModel>($"Auditoria/prestamo/{prestamoId}")
            };

            return View(modelo);
        }

        // RF11 - Información completa del préstamo (link de detalles).
        [HttpGet]
        public IActionResult Detalle(int prestamoId)
        {
            if (!EsAdministrador()) return Forbid();

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + $"Auditoria/prestamo/{prestamoId}/detalle", VerbosHttp.GET, null, Token);

            var modelo = new PrestamoDetalleViewModel();
            if (respuesta.IsSuccessStatusCode)
            {
                modelo.Prestamo = JsonConvert.DeserializeObject<PrestamoAuditoriaModel>(
                    ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            }
            else
            {
                ViewBag.Error = RespuestaApi.LeerError(respuesta);
            }

            return View(modelo);
        }

        // Helper de lectura: arma la URL, manda el token y deserializa el cuerpo a una lista tipada.
        private List<T> ObtenerLista<T>(string recurso)
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + recurso, VerbosHttp.GET, null, Token);
            if (respuesta.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<T>>(ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            return new List<T>();
        }

        private bool EsAdministrador() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Administrador;
    }
}
