using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class PrestamoException : Exception
    {
        public PrestamoException() { }
        public PrestamoException(string message) : base(message) { }
        public PrestamoException(string message, Exception inner) : base(message, inner) { }
    }
}
