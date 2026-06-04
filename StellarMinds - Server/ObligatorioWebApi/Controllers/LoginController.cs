using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObligatorioWebApi.JWT;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using StellarMinds.DTOs.Mappers;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;

namespace ObligatorioWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ICULogin _cuLogin;
        private readonly IJWTHandler _jwtHandler;

        public LoginController(ICULogin cuLogin, IJWTHandler jwtHandler)
        {
            _cuLogin = cuLogin;
            _jwtHandler = jwtHandler;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] DTOLogin credenciales)
        {
            try
            {
                Usuario usuario = _cuLogin.Login(credenciales.Email, credenciales.Pass);

                DTOUsuario usuarioDto = MapperUsuario.ToDtoUsuario(usuario);
                string token = _jwtHandler.GenerarToken(usuarioDto);

                return Ok(new
                {
                    Token = token,
                    Usuario = usuarioDto
                });
            }
            catch (Exception)
            {
                return Unauthorized(new { Message = "Credenciales inválidas. Reintente." });
            }
        }
    }
}
