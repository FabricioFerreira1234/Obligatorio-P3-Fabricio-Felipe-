using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ObligatorioWebApi.JWT;
using StellarMinds.LogicaAccesoDatos.EF.Repositorios;
using StellarMinds.LogicaAplicacion.CasosUso.CUAuditoria;
using StellarMinds.LogicaAplicacion.CasosUso.CUEquipo;
using StellarMinds.LogicaAplicacion.CasosUso.CUObservacion;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUAuditoria;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUObservacion;
using StellarMinds.LogicaAplicacion.Servicios;
using StellarMinds.LogicaAplicacion.CasosUso.CUPrestamo;
using StellarMinds.LogicaAplicacion.CasosUso.CUUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUPrestamo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using StellarMinds.LogicaNegocio.ValueObjects.VOPrestamo;
using System.Text;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StellarMinds API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token JWT. Ejemplo: 'Bearer eyJhbGc...'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// Base de datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure()));

// Autenticación con token JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opciones =>
{
    opciones.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("SecretTokenKey").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddAuthorization(opciones =>
{
    opciones.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// CORS para que el JavaScript del cliente MVC pueda consultar la WebAPI (RF04 - disponibilidad).
const string PoliticaCorsCliente = "PermitirClienteWebApp";
builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy(PoliticaCorsCliente, politica =>
        politica.WithOrigins("https://localhost:7104", "http://localhost:5210")
                .AllowAnyHeader()
                .AllowAnyMethod());
});

//DI - REPOSITORIOS
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioEquipo, RepositorioEquipo>();
builder.Services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();
builder.Services.AddScoped<IRepositorioAuditoria, RepositorioAuditoria>();
builder.Services.AddScoped<IRepositorioObjetoCeleste, RepositorioObjetoCeleste>();
builder.Services.AddScoped<IRepositorioObservacion, RepositorioObservacion>();

// RF07 - Servicio de evaluación de adecuación. Si hay API key de Gemini configurada se consume el
// servicio de IA real; si no, se cae al evaluador local determinístico (útil en desarrollo/sin red).
var geminiOpciones = builder.Configuration.GetSection("Gemini").Get<GeminiOpciones>();
if (geminiOpciones != null && !string.IsNullOrWhiteSpace(geminiOpciones.ApiKey))
{
    builder.Services.AddSingleton(geminiOpciones);
    builder.Services.AddHttpClient<IServicioEvaluacionAdecuacion, EvaluadorAdecuacionGemini>();
}
else
{
    builder.Services.AddScoped<IServicioEvaluacionAdecuacion, EvaluadorAdecuacionLocal>();
}

//DI - CASOS DE USO
//Usuario
builder.Services.AddScoped<ICUAltaUsuario, CUAltaUsuario>();
builder.Services.AddScoped<ICUObtenerUsuarios, CUObtenerUsuarios>();
builder.Services.AddScoped<ICUEliminar, CUEliminar>();
builder.Services.AddScoped<ICULogin, CULogin>();
//Equipo
builder.Services.AddScoped<ICUAltaTelescopio, CUAltaTelescopio>();
builder.Services.AddScoped<ICUAltaMontura, CUAltaMontura>();
builder.Services.AddScoped<ICUAltaCamara, CUAltaCamara>();
builder.Services.AddScoped<ICUAltaOcular, CUAltaOcular>();
builder.Services.AddScoped<ICUObtenerEquipos, CUObtenerEquipos>();
builder.Services.AddScoped<ICUBajaEquipo, CUBajaEquipo>();
builder.Services.AddScoped<ICUEditarTelescopio, CUEditarTelescopio>();
builder.Services.AddScoped<ICUEditarMontura, CUEditarMontura>();
builder.Services.AddScoped<ICUEditarCamara, CUEditarCamara>();
builder.Services.AddScoped<ICUEditarOcular, CUEditarOcular>();
//Prestamo
builder.Services.AddScoped<ICUAltaPrestamo, CUAltaPrestamo>();
builder.Services.AddScoped<ICUListadoPrestamosSocio, CUListadoPrestamosSocio>();
builder.Services.AddScoped<ICUListadoPrestamosEnPrestamoSocio, CUListadoPrestamosEnPrestamoSocio>();
builder.Services.AddScoped<ICUDevolverPrestamo, CUDevolverPrestamo>();
builder.Services.AddScoped<ICUSociosPorTelescopio, CUSociosPorTelescopio>();
//Observacion (RF07)
builder.Services.AddScoped<ICUObtenerObjetosCelestes, CUObtenerObjetosCelestes>();
builder.Services.AddScoped<ICUObtenerPrestamosVigentesSocio, CUObtenerPrestamosVigentesSocio>();
builder.Services.AddScoped<ICUEvaluarAdecuacion, CUEvaluarAdecuacion>();
builder.Services.AddScoped<ICUAltaObservacion, CUAltaObservacion>();
builder.Services.AddScoped<ICURankingObjetosCelestes, CURankingObjetosCelestes>(); // RF10
//Auditoria (RF11)
builder.Services.AddScoped<ICUListadoPrestamosAuditoria, CUListadoPrestamosAuditoria>();
builder.Services.AddScoped<ICUAuditoriaPrestamo, CUAuditoriaPrestamo>();
builder.Services.AddScoped<ICUDetallePrestamo, CUDetallePrestamo>();

// Manejador de tokens JWT
builder.Services.AddScoped<IJWTHandler, JWTHandler>();

var app = builder.Build();

// Arranca el motor de LocalDB antes de conectar para evitar el error 50 por arranque en frio
// (y que el depurador rompa en la SqlException de primer chance).
IniciarLocalDb();

// Aplica las migraciones pendientes al arrancar (crea la base si no existe) y precarga datos.
// Se reintenta como red de seguridad por si LocalDB aun no termino de arrancar.
using (var scope = app.Services.CreateScope())
{
    var contexto = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    const int intentosMaximos = 5;
    for (var intento = 1; ; intento++)
    {
        try
        {
            contexto.Database.Migrate();
            break;
        }
        catch (SqlException) when (intento < intentosMaximos)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
    CargarDatosIniciales(contexto);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(PoliticaCorsCliente);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void IniciarLocalDb()
{
    try
    {
        var inicio = new System.Diagnostics.ProcessStartInfo("sqllocaldb", "start MSSQLLocalDB")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        using var proceso = System.Diagnostics.Process.Start(inicio);
        proceso?.WaitForExit(15000);
    }
    catch
    {
        // sqllocaldb puede no estar en PATH o el server no ser LocalDB: el reintento de Migrate cubre ese caso.
    }
}

static void CargarDatosIniciales(ApplicationDbContext contexto)
{
    if (!contexto.Usuarios.Any())
    {
        contexto.Usuarios.AddRange(
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("fabricio", "ferreira"),
                Direccion = new DireccionVO("Av. Principal 1000", "Montevideo"),
                Telefono = 99000001,
                Email = "fabricioferreira1169@stellar.com",
                Username = "fabricio",
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Administrador
            },
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("María", "González"),
                Direccion = new DireccionVO("Bulevar Artigas 1234", "Montevideo"),
                Telefono = 99000002,
                Email = "maria@stellar.com",
                Username = "maria1",
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Coordinador
            },
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Juan", "Pérez"),
                Direccion = new DireccionVO("Calle 18 2500", "Montevideo"),
                Telefono = 99000003,
                Email = "JuanPerez@stellar.com",
                Username = "juan1",
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Socio
            },
            // Segundo socio: sirve para verificar el aislamiento del RF08 (cada socio ve SOLO sus préstamos).
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Ana", "Soto"),
                Direccion = new DireccionVO("Av. Italia 4500", "Montevideo"),
                Telefono = 99000004,
                Email = "AnaSoto@stellar.com",
                Username = "ana1",
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Socio
            });

        // Usuarios adicionales para pruebas (todos con pass "1234!"):
        // 9 administradores (admin1..admin9), 9 coordinadores (coord1..coord9) y 10 socios (socio1..socio10).
        for (int i = 1; i <= 9; i++)
        {
            contexto.Usuarios.Add(new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Admin" + i, "Prueba"),
                Direccion = new DireccionVO("Calle Admin " + i, "Montevideo"),
                Telefono = 99100000 + i,
                Email = "admin" + i + "@stellar.com",
                Username = "admin" + i,
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Administrador
            });
        }
        for (int i = 1; i <= 9; i++)
        {
            contexto.Usuarios.Add(new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Coord" + i, "Prueba"),
                Direccion = new DireccionVO("Calle Coord " + i, "Montevideo"),
                Telefono = 99200000 + i,
                Email = "coord" + i + "@stellar.com",
                Username = "coord" + i,
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Coordinador
            });
        }
        for (int i = 1; i <= 10; i++)
        {
            contexto.Usuarios.Add(new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Socio" + i, "Prueba"),
                Direccion = new DireccionVO("Calle Socio " + i, "Montevideo"),
                Telefono = 99300000 + i,
                Email = "socio" + i + "@stellar.com",
                Username = "socio" + i,
                Pass = "1234!",
                TipoUsuario = TipoUsuario.Socio
            });
        }
        contexto.SaveChanges();
    }

    if (!contexto.Equipos.Any())
    {
        contexto.Telescopios.Add(new Telescopio
        {
            Marca = "Celestron",
            Modelo = "NexStar 8SE",
            Cantidad = 2,
            Apertura = 203.2m,
            RelacionFocal = 10m,
            DistanciaFocal = 2032m,
            Peso = 5
        });
        // Segundo telescopio: usado en préstamos por varios socios para probar el RF09 (distinct + orden).
        contexto.Telescopios.Add(new Telescopio
        {
            Marca = "Sky-Watcher",
            Modelo = "Explorer 150P",
            Cantidad = 3,
            Apertura = 150m,
            RelacionFocal = 5m,
            DistanciaFocal = 750m,
            Peso = 4
        });
        // Tercer telescopio: SIN préstamos, para probar el caso "ningún socio" del RF09.
        contexto.Telescopios.Add(new Telescopio
        {
            Marca = "Meade",
            Modelo = "LX90",
            Cantidad = 1,
            Apertura = 203.2m,
            RelacionFocal = 10m,
            DistanciaFocal = 2000m,
            Peso = 6
        });
        contexto.Monturas.Add(new Montura
        {
            Marca = "Sky-Watcher",
            Modelo = "EQ6-R Pro",
            Cantidad = 1,
            Tipo = TipoMontura.Ecuatorial,
            PesoMaximo = 20,
            Goto = true
        });
        contexto.Camaras.Add(new Camara
        {
            Marca = "ZWO",
            Modelo = "ASI294MC",
            Cantidad = 3,
            Sensor = TipoSensor.CMOS,
            Resolucion = 11.7m,
            PixelSize = 4.63m
        });
        contexto.SaveChanges();
    }

    if (!contexto.ObjetosCelestes.Any())
    {
        // Objetos celestes preingresados (en el sistema real se generan con IA). Magnitud aparente real.
        contexto.ObjetosCelestes.AddRange(
            new ObjetoCeleste { Nombre = "Júpiter", Tipo = TipoObjetoCeleste.Planeta, Magnitud = -2.20m },
            new ObjetoCeleste { Nombre = "Saturno", Tipo = TipoObjetoCeleste.Planeta, Magnitud = 0.46m },
            new ObjetoCeleste { Nombre = "Marte", Tipo = TipoObjetoCeleste.Planeta, Magnitud = -1.00m },
            new ObjetoCeleste { Nombre = "Venus", Tipo = TipoObjetoCeleste.Planeta, Magnitud = -4.40m },
            new ObjetoCeleste { Nombre = "Galaxia de Andrómeda (M31)", Tipo = TipoObjetoCeleste.Galaxia, Magnitud = 3.44m },
            new ObjetoCeleste { Nombre = "Galaxia del Remolino (M51)", Tipo = TipoObjetoCeleste.Galaxia, Magnitud = 8.40m },
            new ObjetoCeleste { Nombre = "Nebulosa de Orión (M42)", Tipo = TipoObjetoCeleste.Nebulosa, Magnitud = 4.00m },
            new ObjetoCeleste { Nombre = "Nebulosa del Anillo (M57)", Tipo = TipoObjetoCeleste.Nebulosa, Magnitud = 8.80m },
            new ObjetoCeleste { Nombre = "Polaris", Tipo = TipoObjetoCeleste.Estrella, Magnitud = 1.98m },
            new ObjetoCeleste { Nombre = "Sirio", Tipo = TipoObjetoCeleste.Estrella, Magnitud = -1.46m },
            new ObjetoCeleste { Nombre = "Betelgeuse", Tipo = TipoObjetoCeleste.Estrella, Magnitud = 0.42m });
        contexto.SaveChanges();
    }

    if (!contexto.Prestamos.Any())
    {
        var socio = contexto.Usuarios.First(u => u.Email == "JuanPerez@stellar.com");
        var socio2 = contexto.Usuarios.First(u => u.Email == "AnaSoto@stellar.com");
        var socio3 = contexto.Usuarios.First(u => u.Email == "socio1@stellar.com");
        var telescopio = contexto.Telescopios.First();
        // Segundo telescopio (Sky-Watcher Explorer 150P) para los casos del RF09.
        var telescopio2 = contexto.Telescopios.First(t => t.Modelo == "Explorer 150P");
        var montura = contexto.Monturas.First();
        var camara = contexto.Camaras.First();

        // Casos de prueba del RF08 (listado de préstamos del socio por mes/año + marca de atraso):
        contexto.Prestamos.AddRange(
            // [Juan] MAYO 2026 - atrasado: el fin (2026-05-10) ya pasó y sigue EN_PRESTAMO.
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 5, 1), new DateTime(2026, 5, 10)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = camara.Id,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.EN_PRESTAMO
            },
            // [Juan] MAYO 2026 - devuelto: nunca se marca como atrasado aunque el fin haya pasado.
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 5, 15), new DateTime(2026, 5, 20)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.DEVUELTO
            },
            // [Juan] JUNIO 2026 - atrasado en el mes actual: fin (2026-06-02) ya pasó y sigue EN_PRESTAMO.
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 6, 1), new DateTime(2026, 6, 2)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.EN_PRESTAMO
            },
            // [Juan] JUNIO 2026 - vigente NO atrasado (fin futuro): además sirve para observaciones (RF07).
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 6, 1), new DateTime(2026, 6, 30)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = camara.Id,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.EN_PRESTAMO
            },
            // [Ana] JUNIO 2026 - de OTRO socio: Juan NO debe verlo (aislamiento del RF08).
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 6, 3), new DateTime(2026, 6, 25)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio2.Id,
                Estado = EstadoPrestamo.EN_PRESTAMO
            });

        // Casos de prueba del RF09 (socios que solicitaron un telescopio dado, sin repetir, orden desc por nombre)
        // sobre el SEGUNDO telescopio (Explorer 150P). Socio1 lo pide 2 veces -> debe aparecer UNA sola vez.
        contexto.Prestamos.AddRange(
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 4, 1), new DateTime(2026, 4, 10)),
                TelescopioId = telescopio2.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio3.Id,   // Socio1
                Estado = EstadoPrestamo.DEVUELTO
            },
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 4, 15), new DateTime(2026, 4, 20)),
                TelescopioId = telescopio2.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio3.Id,   // Socio1 otra vez (mismo telescopio): no debe duplicarse en el listado
                Estado = EstadoPrestamo.DEVUELTO
            },
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 4, 5), new DateTime(2026, 4, 12)),
                TelescopioId = telescopio2.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio.Id,    // Juan
                Estado = EstadoPrestamo.DEVUELTO
            },
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 4, 18), new DateTime(2026, 4, 28)),
                TelescopioId = telescopio2.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio2.Id,   // Ana
                Estado = EstadoPrestamo.DEVUELTO
            });
        contexto.SaveChanges();
    }

    if (!contexto.Observaciones.Any())
    {
        // Casos de prueba del RF10 (ranking de objetos celestes observados, orden desc por cantidad):
        // Júpiter 3 veces, Saturno 2 veces, Nebulosa de Orión 1 vez. El resto de los objetos NO se observa,
        // así que NO deben aparecer en el ranking. Las observaciones se cuelgan de préstamos ya existentes.
        var prestamos = contexto.Prestamos.OrderBy(p => p.Id).ToList();
        var jupiter = contexto.ObjetosCelestes.First(o => o.Nombre == "Júpiter");
        var saturno = contexto.ObjetosCelestes.First(o => o.Nombre == "Saturno");
        var orion = contexto.ObjetosCelestes.First(o => o.Nombre == "Nebulosa de Orión (M42)");

        Observacion NuevaObservacion(int prestamoId, ObjetoCeleste objeto, DateTime fecha) => new Observacion
        {
            PrestamoId = prestamoId,
            ObjetoCelesteId = objeto.Id,
            Fecha = fecha,
            ResultadoIA = "ADECUADO",
            MotivoIA = "Observación de prueba precargada (RF10)."
        };

        contexto.Observaciones.AddRange(
            // Júpiter: 3 observaciones -> primero en el ranking.
            NuevaObservacion(prestamos[0].Id, jupiter, new DateTime(2026, 5, 2)),
            NuevaObservacion(prestamos[0].Id, jupiter, new DateTime(2026, 5, 16)),
            NuevaObservacion(prestamos[1].Id, jupiter, new DateTime(2026, 6, 1)),
            // Saturno: 2 observaciones -> segundo en el ranking.
            NuevaObservacion(prestamos[1].Id, saturno, new DateTime(2026, 6, 3)),
            NuevaObservacion(prestamos[2].Id, saturno, new DateTime(2026, 6, 5)),
            // Nebulosa de Orión: 1 observación -> tercero en el ranking.
            NuevaObservacion(prestamos[2].Id, orion, new DateTime(2026, 6, 6)));
        contexto.SaveChanges();
    }
}
