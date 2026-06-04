using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StellarMinds.WebApp.Auxiliar;
using StellarMinds.WebApp.Models;
using StellarMinds.WebApp.Models.Api;
using System.Security.Claims;

namespace StellarMinds.WebApp.Controllers
{
    // Login de la WebApp: autentica contra la WebAPI por HTTP (ClienteHttpAuxiliar). No accede a la base ni al dominio.
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string pass)
        {
            // Login: se envía el cuerpo (objeto LoginModel) SIN token (todavía no hay).
            HttpResponseMessage respuesta = ClienteHttpAuxiliar.EnviarSolicitud(
                ClienteHttpAuxiliar.UrlBase + "Login", VerbosHttp.POST, new LoginModel { Email = email, Pass = pass });

            if (!respuesta.IsSuccessStatusCode)
            {
                ViewBag.Error = RespuestaApi.LeerError(respuesta);
                return View();
            }

            RespuestaLoginApi datos = JsonConvert.DeserializeObject<RespuestaLoginApi>(
                ClienteHttpAuxiliar.ObtenerBody(respuesta));

            if (datos?.Token == null || datos.Usuario == null)
            {
                ViewBag.Error = "Credenciales inválidas. Reintente.";
                return View();
            }

            // Flujo seguro: el token JWT se guarda en sesión y viajará en cada llamada a la API.
            HttpContext.Session.SetString("token", datos.Token);

            // Claims de la cookie de la WebApp, armados con los datos que devolvió la API.
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, datos.Usuario.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, datos.Usuario.NombreCompleto?.Nombre ?? string.Empty),
                new Claim("TipoUsuario", datos.Usuario.TipoUsuario.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
