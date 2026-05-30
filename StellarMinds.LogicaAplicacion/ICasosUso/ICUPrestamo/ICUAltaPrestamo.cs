using StellarMinds.DTOs.DataTransferObjects.DTOsPrestamo;
using StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo;
using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo
{
    public interface ICUAltaPrestamo
    {
        void Ejecutar(DTOAltaPrestamo dto, string email);
    }
}
