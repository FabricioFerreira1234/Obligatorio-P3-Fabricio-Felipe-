using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaAccesoDatos;
using StellarMinds.LogicaAccesoDatos.EF.Repositorios;
using StellarMinds.LogicaAplicacion.CasosUso.CUEquipo;
using StellarMinds.LogicaAplicacion.CasosUso.CUUsuario;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo;
using StellarMinds.LogicaAplicacion.ICasosUso.ICUUsuario;
using StellarMinds.LogicaNegocio.Entidades;
using StellarMinds.LogicaNegocio.Enumeraciones;
using StellarMinds.LogicaNegocio.IRepositorios;
using StellarMinds.LogicaNegocio.ValueObjects.VOUsuario;
using static StellarMinds.LogicaAplicacion.ICasosUso.ICUEquipo.ICUAltaEquipo;

namespace StellarMinds.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login/Index";      // redirige si no está logueado
            options.AccessDeniedPath = "/Login/Index";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        });
            // Add services to the container.
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();
           // builder.Services.AddSession();

            //DI - REPOSITORIOS
            builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            builder.Services.AddScoped<IRepositorioEquipo, RepositorioEquipo>();

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



            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                SeedUsuarios(context);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseSession();        
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
            private static void SeedUsuarios(ApplicationDbContext context)
            {
            if (!context.Usuarios.Any()) // solo inserta si la tabla está vacía
            {
                context.Usuarios.AddRange(
                    new Usuario
                    {
                        NombreCompleto = new NombreCompletoVO("Felipe", "Beriau"),
                        Direccion = new DireccionVO("Av. Principal", "Esquina 1"),
                        Telefono = 099000001,
                        Email = "Felipe@stellar.com",
                        Username = "feli",
                        Pass = "123",
                        TipoUsuario = TipoUsuario.Administrador
                    },
                    new Usuario
                    {
                        NombreCompleto = new NombreCompletoVO("Juan", "Pérez"),
                        Direccion = new DireccionVO("Calle 18", "Calle 25"),
                        Telefono = 099000002,
                        Email = "user@stellar.com",
                        Username = "juanp",
                        Pass = "User123",
                        TipoUsuario = TipoUsuario.Socio
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

