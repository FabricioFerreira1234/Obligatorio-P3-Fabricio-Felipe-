using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class EquipoException : Exception
    {
        public EquipoException() { }
        public EquipoException(string message) : base(message) { }
        public EquipoException(string message, Exception inner) : base(message, inner) { }
    }
}
