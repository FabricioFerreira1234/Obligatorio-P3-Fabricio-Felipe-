using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion
{
    // RF07 - Préstamos no devueltos y vigentes del socio logueado (candidatos a observar).
    public interface ICUObtenerPrestamosVigentesSocio
    {
        List<DTOPrestamoListado> Ejecutar(string email);
    }
}
