using StellarMinds.WebApp.Models;

namespace StellarMinds.WebApp.Models
{
    // RF09 - Listado de socios que solicitaron un telescopio dado (Roles Administrador y Coordinador).
    public class SociosPorTelescopioViewModel
    {
        public int TelescopioId { get; set; }
        // Indica si ya se eligió un telescopio (para mostrar resultados o el mensaje "ningún socio").
        public bool Busco { get; set; }
        public List<TelescopioModel> Telescopios { get; set; } = new();
        public List<SocioTelescopioModel> Socios { get; set; } = new();
    }
}
