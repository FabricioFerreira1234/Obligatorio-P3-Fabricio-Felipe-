using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.ValueObjects.VOPrestamo
{
    [Owned]
    public record FechaVO : IValidable
    {
        public DateTime Inicio { get;  init; }
        public DateTime Fin { get;  init; }

        private FechaVO(DateTime inicio, DateTime fin)
        {
         
            Inicio = inicio;
            Fin = fin;
        }
        public void Validar()
        {
            if (Inicio > Fin)
            {
                throw new PrestamoException("La fecha de inicio no puede ser posterior a la fecha de fin.");
            }
        }
    }
}
