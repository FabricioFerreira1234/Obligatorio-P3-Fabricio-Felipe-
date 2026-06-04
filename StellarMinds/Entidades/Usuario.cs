using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StellarMinds.LogicaNegocio.Entidades
{
    public class Usuario : IValidable
    {
        
      
        public int Id { get; set; }
     
        public NombreCompletoVO NombreCompleto { get; set; }
        public DireccionVO Direccion { get; set; }
        public int Telefono {  get; set; }
        [Required]
        public string Email { get; set; }
       [StringLength(50, MinimumLength = 5, ErrorMessage = "El nombre de usuario debe tener entre 5 y 50 caracteres.")]
        public string Username { get; set; }
        public string Pass { get; set; }
        public TipoUsuario TipoUsuario { get; set; }

        public Usuario() { }
        public Usuario(int id, NombreCompletoVO nombreCompleto, DireccionVO direccion, int telefono, string email, string username, string pass, TipoUsuario tu)
        {
            this.Id = id;
            this.NombreCompleto = nombreCompleto;
            this.Direccion = direccion;
            this.Telefono = telefono;
            this.Email = email;
            this.Username = username;
            this.Pass = pass;
            this.TipoUsuario = tu;
        }

      
        public void Validar()
        {
            if (NombreCompleto.Nombre is null)
            {
                throw new UsuarioException("Nombre vacio");
            }
            if (NombreCompleto.Apellido is null)
            {
                throw new UsuarioException("Apellido vacio");
            }
            if (Direccion.Calle1 is null)
            {
                throw new UsuarioException("Calle 1 vacia");
            }
            if (Direccion.Calle2 is null)
            {
                throw new UsuarioException("Calle 2 vacia");
            }
            if (Telefono < 0) throw new UsuarioException("Telefono invalido");


        }
     
    }
}
