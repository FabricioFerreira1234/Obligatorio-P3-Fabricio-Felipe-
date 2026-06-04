using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class VisualException : Exception
    {
        public VisualException() { }
        public VisualException(string message) : base(message) { }
        public VisualException(string message, Exception inner) : base(message, inner) { }
    }
}
