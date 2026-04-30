using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class ObjetoCelesteException : Exception
    {
        public ObjetoCelesteException() { }
        public ObjetoCelesteException(string message) : base(message) { }
        public ObjetoCelesteException(string message, Exception inner) : base(message, inner) { }
    }
}
