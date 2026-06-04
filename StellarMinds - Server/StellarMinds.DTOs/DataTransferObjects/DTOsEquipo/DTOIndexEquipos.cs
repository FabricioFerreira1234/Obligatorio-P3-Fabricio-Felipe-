using StellarMinds.LogicaNegocio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace StellarMinds.DTOs.DataTransferObjects.DTOsEquipo
{
    public class DTOIndexEquipos
    {
        public List<Telescopio> Telescopios { get; set; } = new();
        public List<Montura> Monturas { get; set; } = new();
        public List<Camara> Camaras { get; set; } = new();
        public List<Ocular> Oculares { get; set; } = new();
    }
}
