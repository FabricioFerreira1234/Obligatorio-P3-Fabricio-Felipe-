using StellarMinds.DTOs.DataTransferObjects.DTOsUsuario;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo
{
    // RF09 - Socios que solicitaron un telescopio dado (sin repetir, orden desc por nombre).
    public interface ICUSociosPorTelescopio
    {
        List<DTOSocioTelescopio> Ejecutar(int telescopioId);
    }
}
