using StellarMinds.WebApp.Enums;

namespace StellarMinds.WebApp.Models
{
    // Modelos de contrato de Equipo. GET api/Equipo devuelve las cuatro listas; los Alta* son cuerpos de POST/PUT.

    public class TelescopioModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Apertura { get; set; }
        public decimal RelacionFocal { get; set; }
        public decimal DistanciaFocal { get; set; }
        public int Peso { get; set; }
    }

    public class MonturaModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoMontura Tipo { get; set; }
        public int PesoMaximo { get; set; }
        public bool Goto { get; set; }
    }

    public class CamaraModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoSensor Sensor { get; set; }
        public decimal Resolucion { get; set; }
        public decimal PixelSize { get; set; }
    }

    public class OcularModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Diametro { get; set; }
        public decimal AnguloVisual { get; set; }
    }

    // Equivale a DTOIndexEquipos (respuesta de GET api/Equipo).
    public class IndexEquiposModel
    {
        public List<TelescopioModel> Telescopios { get; set; } = new();
        public List<MonturaModel> Monturas { get; set; } = new();
        public List<CamaraModel> Camaras { get; set; } = new();
        public List<OcularModel> Oculares { get; set; } = new();
    }

    // Cuerpos de alta/edición (equivalen a los DTOAlta* del servidor).
    public class AltaTelescopioModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Apertura { get; set; }
        public decimal RelacionFocal { get; set; }
        public decimal DistanciaFocal { get; set; }
        public int Peso { get; set; }
    }

    public class AltaMonturaModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoMontura Tipo { get; set; }
        public int PesoMaximo { get; set; }
        public bool Goto { get; set; }
    }

    public class AltaCamaraModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public TipoSensor Sensor { get; set; }
        public decimal Resolucion { get; set; }
        public decimal PixelSize { get; set; }
    }

    public class AltaOcularModel
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Cantidad { get; set; }
        public decimal Diametro { get; set; }
        public decimal AnguloVisual { get; set; }
    }
}
