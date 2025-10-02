using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Data;

namespace CapaDatos.DataContext;

public partial class OlympusContext : DbContext
{
    public OlympusContext(DbContextOptions<OlympusContext> options)
        : base(options)
    { }

    public virtual DbSet<Usuario> Usuario { get; set; }

    public virtual DbSet<UserToken> UserToken { get; set; }
    public virtual DbSet<ErrorLog> ErrorLogs { get; set; }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(Database.GetConnectionString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC071849447B");

            entity.HasIndex(e => e.Correo).IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Correo).HasMaxLength(150);
            entity.Property(e => e.FechaModificacion)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion)
                  .HasDefaultValueSql("(getdate())")
                  .HasColumnType("datetime");
            entity.Property(e => e.Nombre).IsRequired();
            entity.Property(e => e.UsuarioCreacion).HasColumnType("int");
            entity.Property(e => e.UsuarioModificacion).HasColumnType("int");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        // Configuración de ErrorLog
        modelBuilder.Entity<ErrorLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ErrorLog__3214EC07");

            entity.Property(e => e.FechaHora)
                  .HasDefaultValueSql("GETUTCDATE()")
                  .HasColumnType("datetime2");

            entity.Property(e => e.Origen)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(e => e.Mensaje)
                  .IsRequired();

            entity.Property(e => e.Traza);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
