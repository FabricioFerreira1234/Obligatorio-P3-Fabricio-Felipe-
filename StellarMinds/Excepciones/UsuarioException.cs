using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.LogicaNegocio.Excepciones
{
    public class UsuarioException : Exception
    {
        public UsuarioException() { }
        public UsuarioException(string message) : base(message) { }
        public UsuarioException(string message, Exception inner) : base(message, inner) { }
    }
}