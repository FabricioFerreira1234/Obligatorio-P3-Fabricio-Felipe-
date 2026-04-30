using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class ObservacionException : Exception
    {
        public ObservacionException() { }
        public ObservacionException(string message) : base(message) { }
                        
        public ObservacionException(string message, Exception inner) : base(message, inner) { }
    }
}
