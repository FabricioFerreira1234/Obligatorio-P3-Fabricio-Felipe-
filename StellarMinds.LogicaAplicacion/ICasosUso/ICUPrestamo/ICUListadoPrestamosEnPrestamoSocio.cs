using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo
{
    public interface ICUListadoPrestamosEnPrestamoSocio
    {
        List<DTOPrestamoListado> Ejecutar(int socioId);
    }
}
