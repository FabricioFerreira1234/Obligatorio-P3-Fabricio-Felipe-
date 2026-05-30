using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ObligatorioWebApi.JWT;
using StellarMinds.LogicaAccesoDatos;
using StellarMinds.LogicaAccesoDatos.EF.Repositorios;
using StellarMinds.LogicaAplicacion.CasosUso.CUEquipo;
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

//DI - REPOSITORIOS
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioEquipo, RepositorioEquipo>();
builder.Services.AddScoped<IRepositorioPrestamo, RepositorioPrestamo>();

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
                NombreCompleto = new NombreCompletoVO("Felipe", "Beriau"),
                Direccion = new DireccionVO("Av. Principal 1000", "Montevideo"),
                Telefono = 99000001,
                Email = "felipe@stellar.com",
                Username = "feli1",
                Pass = "Admin123!",
                TipoUsuario = TipoUsuario.Administrador
            },
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("María", "González"),
                Direccion = new DireccionVO("Bulevar Artigas 1234", "Montevideo"),
                Telefono = 99000002,
                Email = "maria@stellar.com",
                Username = "maria1",
                Pass = "Coord123!",
                TipoUsuario = TipoUsuario.Coordinador
            },
            new Usuario
            {
                NombreCompleto = new NombreCompletoVO("Juan", "Pérez"),
                Direccion = new DireccionVO("Calle 18 2500", "Montevideo"),
                Telefono = 99000003,
                Email = "juan@stellar.com",
                Username = "juan1",
                Pass = "User123!",
                TipoUsuario = TipoUsuario.Socio
            });
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

    if (!contexto.Prestamos.Any())
    {
        var socio = contexto.Usuarios.First(u => u.Email == "juan@stellar.com");
        var telescopio = contexto.Telescopios.First();
        var montura = contexto.Monturas.First();
        var camara = contexto.Camaras.First();

        contexto.Prestamos.AddRange(
            new Prestamo
            {
                // Atrasado: el fin (2026-05-10) ya pasó y sigue EN_PRESTAMO.
                Fecha = new FechaVO(new DateTime(2026, 5, 1), new DateTime(2026, 5, 10)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = camara.Id,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.EN_PRESTAMO
            },
            new Prestamo
            {
                Fecha = new FechaVO(new DateTime(2026, 5, 15), new DateTime(2026, 5, 20)),
                TelescopioId = telescopio.Id,
                MonturaId = montura.Id,
                VisualId = null,
                UsuarioId = socio.Id,
                Estado = EstadoPrestamo.DEVUELTO
            });
        contexto.SaveChanges();
    }
}
