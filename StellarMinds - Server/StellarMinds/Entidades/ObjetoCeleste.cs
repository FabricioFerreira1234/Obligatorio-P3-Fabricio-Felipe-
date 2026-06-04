using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;

namespace StellarMinds.LogicaNegocio.Entidades
{
    // Objeto celeste preingresado en el sistema (Luna, Júpiter, Galaxia M42, Polaris...).
    // La magnitud aparente es un decimal que puede ser positivo o negativo, con 2 decimales.
    public class ObjetoCeleste : IValidable
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public TipoObjetoCeleste Tipo { get; set; }
        public decimal Magnitud { get; set; }

        public ObjetoCeleste() { }

        public ObjetoCeleste(int id, string nombre, TipoObjetoCeleste tipo, decimal magnitud)
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
            Magnitud = magnitud;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new ObjetoCelesteException("El nombre del objeto celeste es requerido.");
        }
    }
}
