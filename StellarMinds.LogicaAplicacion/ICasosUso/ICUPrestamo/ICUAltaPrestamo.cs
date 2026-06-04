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
        // Alta hecha por el propio socio (se resuelve el socio por su email logueado).
        void Ejecutar(DTOAltaPrestamo dto, string email);

        // Alta hecha por el Coordinador (el socio viene indicado en dto.UsuarioId).
        // RF06: se audita la acción con el email del coordinador logueado.
        void EjecutarComoCoordinador(DTOAltaPrestamo dto, string emailCoordinador);
    }
}
