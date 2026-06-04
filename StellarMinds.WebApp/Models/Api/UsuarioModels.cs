namespace StellarMinds.WebApp.Models.Api
{
    // Modelos de contrato que la WebApp usa para hablar con la API de Usuario.
    // Reemplazan a los DTOs del servidor: la WebApp ya NO referencia StellarMinds.DTOs ni el dominio.

    // Equivale al value object NombreCompletoVO del servidor (mismo shape JSON).
    public class NombreCompletoModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }

    // Equivale al value object DireccionVO del servidor (mismo shape JSON).
    public class DireccionModel
    {
        public string Calle1 { get; set; }
        public string Calle2 { get; set; }
    }

    // Equivale a DTOUsuario (respuesta de GET api/Usuario y del login).
    public class UsuarioModel
    {
        public int Id { get; set; }
        public NombreCompletoModel NombreCompleto { get; set; } = new();
        public DireccionModel Direccion { get; set; } = new();
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }

    // Equivale a DTOAltaUsuario (cuerpo de POST api/Usuario).
    public class AltaUsuarioModel
    {
        public NombreCompletoModel NombreCompleto { get; set; }
        public DireccionModel Direccion { get; set; }
        public int Telefono { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }

        public AltaUsuarioModel()
        {
            NombreCompleto = new NombreCompletoModel();
            Direccion = new DireccionModel();
        }
    }

    // Equivale a DTOLogin (cuerpo de POST api/Login).
    public class LoginModel
    {
        public string Email { get; set; }
        public string Pass { get; set; }
    }

    // RF09 - Equivale a DTOSocioTelescopio (respuesta de GET api/Prestamo/telescopio/{id}/socios).
    public class SocioTelescopioModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
    }
}
