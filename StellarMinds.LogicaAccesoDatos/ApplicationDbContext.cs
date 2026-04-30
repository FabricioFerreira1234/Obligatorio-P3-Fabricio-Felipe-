using Microsoft.EntityFrameworkCore;
using StellarMinds.LogicaNegocio.Entidades;

public class ApplicationDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Equipo> Equipos { get; set; }
    public DbSet<Prestamo> Prestamos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Herencia TPH Equipo
        modelBuilder.Entity<Equipo>()
            .HasDiscriminator<string>("TipoEquipo")
            .HasValue<Telescopio>("Telescopio")
            .HasValue<Montura>("Montura")
            .HasValue<Camara>("Camara")
            .HasValue<Ocular>("Ocular");

        // Precisiones decimales (las que ya tenías)
        modelBuilder.Entity<Telescopio>(e =>
        {
            e.Property(t => t.Apertura).HasPrecision(10, 2);
            e.Property(t => t.RelacionFocal).HasPrecision(10, 2);
            e.Property(t => t.DistanciaFocal).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Camara>(e =>
        {
            e.Property(c => c.Resolucion).HasPrecision(10, 2);
            e.Property(c => c.PixelSize).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Ocular>(e =>
        {
            e.Property(o => o.Diametro).HasPrecision(10, 2);
            e.Property(o => o.AnguloVisual).HasPrecision(10, 2);
        });

        // Prestamo - FK y relaciones
        modelBuilder.Entity<Prestamo>(e =>
        {
            e.HasOne(p => p.Telescopio)
                .WithMany()
                .HasForeignKey("TelescopioId")
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Montura)
                .WithMany()
                .HasForeignKey("MonturaId")
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(p => p.Visual)
                .WithMany()
                .HasForeignKey("VisualId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            e.Property(p => p.Estado)
                .HasConversion<string>();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=StellarMindsDB;IntegratedSecurity=True;");
        }
        base.OnConfiguring(optionsBuilder);
    }
}