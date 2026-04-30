using StellarMinds.DTOs.DataTransferObjects.DTOsEquipo;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo
{
    public class ICUAltaEquipo
    {
        public interface ICUAltaTelescopio { void Alta(DTOAltaTelescopio dto); }
        public interface ICUAltaMontura { void Alta(DTOAltaMontura dto); }
        public interface ICUAltaCamara { void Alta(DTOAltaCamara dto); }
        public interface ICUAltaOcular { void Alta(DTOAltaOcular dto); }
    }
}
