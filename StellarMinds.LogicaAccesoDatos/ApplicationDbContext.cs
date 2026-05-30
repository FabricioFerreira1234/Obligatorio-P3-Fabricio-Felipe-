using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;

public class ApplicationDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }

    public DbSet<Visual> Visuales { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }


    public DbSet<Ocular> Oculares { get; set; }
    public DbSet <Camara> Camaras { get; set; }
    public DbSet <Telescopio> Telescopios { get; set; }
    public DbSet <Montura> Monturas { get; set; }



    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Camara>().Property(c => c.Resolucion).HasPrecision(10, 2);
        modelBuilder.Entity<Camara>().Property(c => c.PixelSize).HasPrecision(10, 2);

        modelBuilder.Entity<Ocular>().Property(o => o.Diametro).HasPrecision(10, 2);
        modelBuilder.Entity<Ocular>().Property(o => o.AnguloVisual).HasPrecision(10, 2);

        modelBuilder.Entity<Telescopio>().Property(t => t.Apertura).HasPrecision(10, 2);
        modelBuilder.Entity<Telescopio>().Property(t => t.RelacionFocal).HasPrecision(10, 2);
        modelBuilder.Entity<Telescopio>().Property(t => t.DistanciaFocal).HasPrecision(10, 2);

        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Telescopio).WithMany()
            .HasForeignKey(p => p.TelescopioId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Montura).WithMany()
            .HasForeignKey(p => p.MonturaId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Visual).WithMany()
            .HasForeignKey(p => p.VisualId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Prestamo>()
            .HasOne(p => p.Usuario).WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
