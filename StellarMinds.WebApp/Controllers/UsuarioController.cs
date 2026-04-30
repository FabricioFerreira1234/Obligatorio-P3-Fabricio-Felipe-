using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.Enumeraciones;

namespace StellarMinds.WebApp.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ICUAltaUsuario _cuAltaUsuario;
        private readonly ICUObtenerUsuarios _cuObtenerUsuarios;
        private readonly ICUEliminar _cuEliminarUsuario;

        public UsuarioController(ICUAltaUsuario cuAltaUsuario,
                                 ICUObtenerUsuarios cuObtenerUsuarios,
                                 ICUEliminar cuEliminarUsuario)
        {
            _cuAltaUsuario = cuAltaUsuario;
            _cuObtenerUsuarios = cuObtenerUsuarios;
            _cuEliminarUsuario = cuEliminarUsuario;
        }

  
        private bool EsAdministrador()
        {
            var tipo = User.FindFirst("TipoUsuario")?.Value;
            return Enum.TryParse<TipoUsuario>(tipo, out var t) && t == TipoUsuario.Administrador;
        }

      
        public IActionResult Index()
        {
            return View(_cuObtenerUsuarios.ObtenerUsuarios());
        }

     
        [Authorize]
        public IActionResult Create()
        {
            if (!EsAdministrador()) return Forbid();
            return View();
        }

      
        [HttpPost]
        [Authorize]
        public IActionResult Create(DTOAltaUsuario u)
        {
            if (!EsAdministrador()) return Forbid();
            if (!ModelState.IsValid) return View(u);

            try
            {
                _cuAltaUsuario.AltaUsuario(u);
            }
            catch (UsuarioException e)
            {
                ViewBag.Error = e.Message; // así la vista puede mostrar el error
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Error = "Ocurrió un error inesperado.";
                return View();
            }

            return RedirectToAction("Index");
        }

       
        public IActionResult Delete(string email)
        {
            _cuEliminarUsuario.Eliminar(email);
            return RedirectToAction("Index");
        }
    }
}