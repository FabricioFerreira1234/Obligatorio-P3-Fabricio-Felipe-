namespace StellarMinds.DTOs.DataTransferObjects.DTOsObjetoCeleste
{
    // RF10 - Fila del ranking de objetos celestes observados: nombre, tipo y cantidad de veces observado.
    public class DTORankingObjetoCeleste
    {
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int CantidadObservaciones { get; set; }
    }
}
