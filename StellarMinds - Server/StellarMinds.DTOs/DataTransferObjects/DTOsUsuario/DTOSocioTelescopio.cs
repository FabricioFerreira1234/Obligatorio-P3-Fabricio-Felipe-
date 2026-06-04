namespace StellarMinds.DTOs.DataTransferObjects.DTOsUsuario
{
    // RF09 - Socio que solicitó un telescopio dado (datos mínimos para el listado, sin exponer la contraseña).
    public class DTOSocioTelescopio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
    }
}
