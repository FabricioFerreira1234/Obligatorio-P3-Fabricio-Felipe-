using Microsoft.IdentityModel.Tokens;
using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ObligatorioWebApi.JWT
{
    public interface IJWTHandler
    {
        string GenerarToken(DTOUsuario usuarioDto);
    }

    public class JWTHandler : IJWTHandler
    {
        private readonly IConfiguration _configuration;

        public JWTHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(DTOUsuario usuarioDto)
        {
            var manejadorToken = new JwtSecurityTokenHandler();

            var clave = Encoding.UTF8.GetBytes(_configuration.GetSection("SecretTokenKey").Value!);

            var afirmaciones = new Claim[]
            {
                new Claim(ClaimTypes.Name, usuarioDto.Username),
                new Claim(ClaimTypes.Email, usuarioDto.Email),
                new Claim(ClaimTypes.Role, usuarioDto.TipoUsuario.ToString())
            };

            var credenciales = new SigningCredentials(
                new SymmetricSecurityKey(clave),
                SecurityAlgorithms.HmacSha256Signature);

            var descriptorToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(afirmaciones),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = credenciales
            };

            var token = manejadorToken.CreateToken(descriptorToken);

            return manejadorToken.WriteToken(token);
        }
    }
}
