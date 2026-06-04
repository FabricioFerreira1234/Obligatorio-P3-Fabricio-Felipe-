using StellarMinds.LogicaNegocio.Excepciones;
using StellarMinds.LogicaNegocio.InterfacesDominio;
using System;

namespace StellarMinds.LogicaNegocio.Entidades
{
    // RF07 - Observación astronómica planificada por un socio: vincula un préstamo vigente
    // con un objeto celeste en una fecha, y guarda el resultado de la evaluación de adecuación.
    public class Observacion : IValidable
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }

        public int PrestamoId { get; set; }
        public Prestamo Prestamo { get; set; }

        public int ObjetoCelesteId { get; set; }
        public ObjetoCeleste ObjetoCeleste { get; set; }

        // Resultado de la evaluación de adecuación (IDEAL / ADECUADO / NO RECOMENDABLE) y su motivo.
        public string ResultadoIA { get; set; }
        public string MotivoIA { get; set; }

        public Observacion() { }

        public void Validar()
        {
            if (PrestamoId <= 0)
                throw new ObservacionException("Debe seleccionar el préstamo a observar.");

            if (ObjetoCelesteId <= 0)
                throw new ObservacionException("Debe seleccionar el objeto celeste a observar.");
        }
    }
}
