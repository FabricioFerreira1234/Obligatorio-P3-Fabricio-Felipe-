using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Enums;
using StellarMinds.WebApp.Filter;
using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Controllers
{
    // Préstamos: alta por Coordinador (RF04), devolución (RF05), Mis Préstamos (RF08) y
    // Socios por Telescopio (RF09). Consume la WebAPI vía ClienteHttpAuxiliar (token Bearer).
    [LoginFilter]
    public class PrestamoController : Controller
    {
        private string Token => HttpContext.Session.GetString("token");

        [HttpGet]
        public IActionResult Create()
        {
            if (!EsCoordinador()) return Forbid();

            var modelo = new PrestamoCreateViewModel();
            CargarListas(modelo);
            return View(modelo);
        }

        [HttpPost]
        public IActionResult Create(PrestamoCreateViewModel modelo)
        {
            if (!EsCoordinador()) return Forbid();

            var dto = new AltaPrestamoModel
            {
                Fecha = new FechaModel { Inicio = modelo.FechaInicio, Fin = modelo.FechaFin },
                TelescopioId = modelo.TelescopioId,
                MonturaId = modelo.MonturaId,
                VisualId = modelo.VisualId,
                UsuarioId = modelo.SocioId,
                Estado = EstadoPrestamo.EN_PRESTAMO
            };

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Prestamo/coordinador", VerbosHttp.POST, dto, Token);

            if (respuesta.IsSuccessStatusCode)
            {
                TempData["Exito"] = "Préstamo registrado correctamente.";
                return RedirectToAction("Create");
            }

            ViewBag.Error = RespuestaApi.LeerError(respuesta);
            CargarListas(modelo);
            return View(modelo);
        }

        // RF05 - El Coordinador elige un socio y se listan sus préstamos EN PRÉSTAMO para devolver uno.
        [HttpGet]
        public IActionResult Devolucion(int? socioId)
        {
            if (!EsCoordinador()) return Forbid();

            var modelo = new PrestamoDevolucionViewModel
            {
                Socios = ObtenerLista<UsuarioModel>("Prestamo/socios")
            };

            if (socioId.HasValue && socioId.Value > 0)
            {
                modelo.SocioId = socioId.Value;
                modelo.BuscoSocio = true;
                modelo.Prestamos = ObtenerLista<PrestamoListadoModel>($"Prestamo/socio/{socioId.Value}/activos");
            }

            return View(modelo);
        }

        [HttpPost]
        public IActionResult Devolver(int prestamoId, int socioId)
        {
            if (!EsCoordinador()) return Forbid();

            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + $"Prestamo/{prestamoId}/devolver", VerbosHttp.PUT, null, Token);

            TempData[respuesta.IsSuccessStatusCode ? "Exito" : "Error"] =
                respuesta.IsSuccessStatusCode ? "Devolución registrada correctamente." : RespuestaApi.LeerError(respuesta);

            return RedirectToAction("Devolucion", new { socioId });
        }

        // RF08 - Listado de los préstamos del socio logueado en un mes y año dados.
        [HttpGet]
        public IActionResult MisPrestamos(int? mes, int? anio)
        {
            if (!EsSocio()) return Forbid();

            var modelo = new MisPrestamosViewModel
            {
                Mes = mes ?? DateTime.Today.Month,
                Anio = anio ?? DateTime.Today.Year
            };

            if (mes.HasValue && anio.HasValue)
            {
                modelo.Busco = true;
                modelo.Prestamos = ObtenerLista<PrestamoListadoModel>($"Prestamo?mes={modelo.Mes}&anio={modelo.Anio}");
            }

            return View(modelo);
        }

        // RF09 - Listado de socios a los que se les prestó un telescopio dado (Administrador y Coordinador).
        [HttpGet]
        public IActionResult SociosPorTelescopio(int? telescopioId)
        {
            if (!EsAdminOCoordinador()) return Forbid();

            var modelo = new SociosPorTelescopioViewModel
            {
                Telescopios = ObtenerEquipos().Telescopios
            };

            if (telescopioId.HasValue && telescopioId.Value > 0)
            {
                modelo.TelescopioId = telescopioId.Value;
                modelo.Busco = true;
                modelo.Socios = ObtenerLista<SocioTelescopioModel>($"Prestamo/telescopio/{telescopioId.Value}/socios");
            }

            return View(modelo);
        }

        private void CargarListas(PrestamoCreateViewModel modelo)
        {
            modelo.Socios = ObtenerLista<UsuarioModel>("Prestamo/socios");
            IndexEquiposModel equipos = ObtenerEquipos();
            modelo.Telescopios = equipos.Telescopios;
            modelo.Monturas = equipos.Monturas;
            modelo.Camaras = equipos.Camaras;
            modelo.Oculares = equipos.Oculares;
        }

        // Helpers de lectura: arman la URL, mandan el token y deserializan el cuerpo.
        private List<T> ObtenerLista<T>(string recurso)
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + recurso, VerbosHttp.GET, null, Token);
            if (respuesta.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<List<T>>(ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            return new List<T>();
        }

        private IndexEquiposModel ObtenerEquipos()
        {
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Equipo", VerbosHttp.GET, null, Token);
            if (respuesta.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<IndexEquiposModel>(ClienteHttpAuxiliar.ObtenerBody(respuesta)) ?? new();
            return new IndexEquiposModel();
        }

        private bool EsCoordinador() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Coordinador;

        private bool EsSocio() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && t == TipoUsuario.Socio;

        private bool EsAdminOCoordinador() =>
            Enum.TryParse<TipoUsuario>(User.FindFirst("TipoUsuario")?.Value, out var t)
            && (t == TipoUsuario.Administrador || t == TipoUsuario.Coordinador);
    }
}
