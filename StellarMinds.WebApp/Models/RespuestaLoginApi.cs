using StellarMinds.WebApp.Models.Api;

namespace StellarMinds.WebApp.Models
{
    // Forma de la respuesta del endpoint POST api/Login de la WebAPI: token JWT + datos del usuario.
    public class RespuestaLoginApi
    {
        public string Token { get; set; }
        public UsuarioModel Usuario { get; set; }
    }
}
