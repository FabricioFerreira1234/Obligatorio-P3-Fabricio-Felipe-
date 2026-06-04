namespace StellarMinds.WebApp.Models.Api
{
    // Enumeraciones propias de la WebApp. Replican los valores (y el orden, por lo tanto el entero
    // subyacente) de las enumeraciones del servidor, para deserializar correctamente el JSON de la API.

    public enum TipoUsuario
    {
        Administrador,
        Coordinador,
        Socio
    }

    public enum TipoMontura
    {
        Ecuatorial,
        Altazimutal,
        Hibrida
    }

    public enum TipoSensor
    {
        CMOS,
        CCD
    }

    public enum TipoObjetoCeleste
    {
        Planeta,
        Galaxia,
        Nebulosa,
        Estrella
    }

    public enum EstadoPrestamo
    {
        EN_PRESTAMO,
        DEVUELTO
    }
}
