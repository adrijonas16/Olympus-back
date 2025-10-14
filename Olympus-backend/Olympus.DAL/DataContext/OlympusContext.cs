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
    public virtual DbSet<Persona> Persona { get; set; }
    public virtual DbSet<Asesor> Asesor { get; set; }
    public virtual DbSet<Estado> Estado { get; set; }
    public virtual DbSet<Motivo> Motivo { get; set; }
    public virtual DbSet<Oportunidad> Oportunidad { get; set; }
    public virtual DbSet<ControlOportunidad> ControlOportunidad { get; set; }
    public virtual DbSet<HistorialEstado> HistorialEstado { get; set; }
    public virtual DbSet<HistorialInteraccion> HistorialInteraccion { get; set; }
    public virtual DbSet<Pais> Pais { get; set; }
    public virtual DbSet<Lanzamiento> Lanzamiento { get; set; }

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

        // Configuración Persona
        modelBuilder.Entity<Persona>(entity =>
        {
            entity.ToTable("Persona", schema: "adm");
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nombres)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Apellidos)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PrefijoPaisCelular)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.AreaTrabajo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Industria)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IdPais)
            .HasColumnName("IdPais");

            entity.HasOne(p => p.Pais)
                .WithMany(pa => pa.Personas)
                .HasForeignKey(p => p.IdPais)
                .HasConstraintName("FK_Persona_Pais")
                .OnDelete(DeleteBehavior.Restrict);

            // índice opcional
            entity.HasIndex(e => e.IdPais).HasDatabaseName("IX_Persona_IdPais");
        });

        // Configuración Asesor
        modelBuilder.Entity<Asesor>(entity =>
        {
            entity.ToTable("Asesor", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombres)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Apellidos)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.PrefijoPaisCelular)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.Property(e => e.Correo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.AreaTrabajo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IdPais)
                .HasColumnName("IdPais");

            entity.HasOne(p => p.Pais)
                .WithMany(pa => pa.Asesores)
                .HasForeignKey(p => p.IdPais)
                .HasConstraintName("FK_Asesor_Pais")
                .OnDelete(DeleteBehavior.Restrict);

            // índice opcional
            entity.HasIndex(e => e.IdPais).HasDatabaseName("IX_Asesor_IdPais");
        });

        // Configuración Estado
        modelBuilder.Entity<Estado>(entity =>
        {
            entity.ToTable("Estado", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.EstadoControl)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");
        });

        // Configuración Motivo
        modelBuilder.Entity<Motivo>(entity =>
        {
            entity.ToTable("Motivo", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        // Configuración Oportunidad
        modelBuilder.Entity<Oportunidad>(entity =>
        {
            entity.ToTable("Oportunidad", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            // Exponer explícitamente la columna IdPersona (asegura que EF la conozca)
            entity.Property(e => e.IdPersona)
                .HasColumnName("IdPersona")
                .IsRequired();

            // Muchas Oportunidad -> 1 Persona, forzar FK usando la propiedad IdPersona
            entity.HasOne(o => o.Persona)
                  .WithMany(p => p.Oportunidades)
                  .HasForeignKey(o => o.IdPersona)
                  .HasConstraintName("FK_Oportunidad_Persona")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índice
            entity.HasIndex(e => e.IdPersona).HasDatabaseName("IX_Oportunidad_IdPersona");

            // FK Lanzamiento.Id
            entity.Property(e => e.IdLanzamiento)
                  .HasColumnName("IdLanzamiento")
                  .IsRequired();

            entity.HasOne(o => o.Lanzamiento)
                  .WithMany(l => l.Oportunidades)
                  .HasForeignKey(o => o.IdLanzamiento)
                  .HasConstraintName("FK_Oportunidad_Lanzamiento")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.IdLanzamiento).HasDatabaseName("IX_Oportunidad_IdLanzamiento");
        });

        // Configuración ControlOportunidad
        modelBuilder.Entity<ControlOportunidad>(entity =>
        {
            entity.ToTable("ControlOportunidad", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Detalle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.IdOportunidad)
              .HasColumnName("IdOportunidad")
              .IsRequired();

            // Relación muchos ControlOportunidad -> 1 Oportunidad
            entity.HasOne(c => c.Oportunidad)
                  .WithMany(o => o.ControlOportunidades)
                  .HasForeignKey(c => c.IdOportunidad)
                  .HasConstraintName("FK_ControlOportunidad_Oportunidad")
                  .OnDelete(DeleteBehavior.Restrict);


            // índice para optimizar búsquedas por IdOportunidad
            entity.HasIndex(e => e.IdOportunidad).HasDatabaseName("IX_ControlOportunidad_IdOportunidad");
        });

        // Configuración HistorialEstado
        modelBuilder.Entity<HistorialEstado>(entity =>
        {
            entity.ToTable("HistorialEstado", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Observaciones)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.CantidadLlamadasContestadas);
            entity.Property(e => e.CantidadLlamadasNoContestadas);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.IdOportunidad).HasColumnName("IdOportunidad").IsRequired();
            entity.Property(e => e.IdAsesor).HasColumnName("IdAsesor").IsRequired(false);
            entity.Property(e => e.IdMotivo).HasColumnName("IdMotivo").IsRequired(false);
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado").IsRequired(false);

            // Relaciones: usar lambdas para forzar uso de propiedades FK
            entity.HasOne(h => h.Oportunidad)
                  .WithMany(o => o.HistorialEstado)
                  .HasForeignKey(h => h.IdOportunidad)
                  .HasConstraintName("FK_HistorialEstado_Oportunidad")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.Asesor)
                  .WithMany(a => a.HistorialEstado)
                  .HasForeignKey(h => h.IdAsesor)
                  .HasConstraintName("FK_HistorialEstado_Asesor")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.Motivo)
                  .WithMany(m => m.HistorialEstado)
                  .HasForeignKey(h => h.IdMotivo)
                  .HasConstraintName("FK_HistorialEstado_Motivo")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.EstadoReferencia)
                  .WithMany(e => e.HistorialEstado)
                  .HasForeignKey(h => h.IdEstado)
                  .HasConstraintName("FK_HistorialEstado_Estado")
                  .OnDelete(DeleteBehavior.Restrict);

            // índices
            entity.HasIndex(h => h.IdOportunidad).HasDatabaseName("IX_HistorialEstado_IdOportunidad");
            entity.HasIndex(h => h.IdAsesor).HasDatabaseName("IX_HistorialEstado_IdAsesor");
            entity.HasIndex(h => h.IdMotivo).HasDatabaseName("IX_HistorialEstado_IdMotivo");
            entity.HasIndex(h => h.IdEstado).HasDatabaseName("IX_HistorialEstado_IdEstado");
        });


        // Configuración HistorialInteraccion
        modelBuilder.Entity<HistorialInteraccion>(entity =>
        {
            entity.ToTable("HistorialInteraccion", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Detalle)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.Tipo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.FechaRecordatorio)
                .HasColumnType("datetime");

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            // FK explícita
            entity.Property(e => e.IdOportunidad).HasColumnName("IdOportunidad").IsRequired();

            entity.HasOne(h => h.Oportunidad)
                  .WithMany(o => o.HistorialInteracciones)
                  .HasForeignKey(h => h.IdOportunidad)
                  .HasConstraintName("FK_HistorialInteraccion_Oportunidad")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índice para optimizar búsquedas por IdOportunidad
            entity.HasIndex("IdOportunidad").HasDatabaseName("IX_HistorialInteraccion_IdOportunidad");
        });

        // Configuración Pais
        modelBuilder.Entity<Pais>(entity =>
        {
            entity.ToTable("Pais", schema: "mdm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .HasMaxLength(150)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.PrefijoCelularPais);

            entity.Property(e => e.DigitoMaximo);

            entity.Property(e => e.DigitoMinimo);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado");

            entity.Property(e => e.UsuarioCreacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.UsuarioModificacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.FechaCreacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IdMigracion).HasColumnName("IdMigracion");

            // RowVersion / timestamp
            entity.Property(e => e.RowVersion)
                  .IsRowVersion()
                  .IsConcurrencyToken()
                  .HasColumnType("rowversion")
                  .HasColumnName("RowVersion");

            // índice opcional si quieres buscar por Nombre
            entity.HasIndex(e => e.Nombre).HasDatabaseName("IX_Pais_Nombre");
        });

        // Configuración Lanzamiento
        modelBuilder.Entity<Lanzamiento>(entity =>
        {
            entity.ToTable("Lanzamiento", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CodigoLanzamiento)
                  .HasMaxLength(255)
                  .IsUnicode(false);

            entity.Property(e => e.Estado).HasColumnName("Estado");

            entity.Property(e => e.FechaCreacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                  .HasMaxLength(50)
                  .IsUnicode(false);
            entity.Property(e => e.UsuarioModificacion)
                  .HasMaxLength(50)
                  .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
