using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using System.Collections.Generic;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo
{
    public interface ICUListadoPrestamosSocio
    {
        List<DTOPrestamoListado> Ejecutar(string email, int mes, int anio);
    }
}
