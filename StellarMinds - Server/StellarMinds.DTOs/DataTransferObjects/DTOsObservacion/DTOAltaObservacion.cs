using System;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsObservacion
{
    // RF07 - Datos que el socio elige para una observación: el préstamo, el objeto celeste y la fecha.
    public class DTOAltaObservacion
    {
        public int PrestamoId { get; set; }
        public int ObjetoCelesteId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
