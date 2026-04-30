using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using System.Security.Claims;

namespace StellarMinds.WebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ICULogin _cuLogin;

        public LoginController(ICULogin cuLogin)
        {
            _cuLogin = cuLogin;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string pass)
        {
            try
            {
                Usuario usuario = _cuLogin.Login(email, pass);

                // Crear los claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Name, usuario.NombreCompleto.Nombre),
                    new Claim("TipoUsuario", usuario.TipoUsuario.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}