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
    public virtual DbSet<Oportunidad> Oportunidad { get; set; }
    public virtual DbSet<Producto> Producto { get; set; }
    public virtual DbSet<ControlOportunidad> ControlOportunidad { get; set; }
    public virtual DbSet<HistorialEstado> HistorialEstado { get; set; }
    public virtual DbSet<HistorialInteraccion> HistorialInteraccion { get; set; }
    public virtual DbSet<Pais> Pais { get; set; }
    public virtual DbSet<Beneficio> Beneficio { get; set; }
    public virtual DbSet<Certificado> Certificado { get; set; }
    public virtual DbSet<Cobranza> Cobranza { get; set; }
    public virtual DbSet<Convertido> Convertido { get; set; }
    public virtual DbSet<Corporativo> Corporativo { get; set; }
    public virtual DbSet<Docente> Docente { get; set; }
    public virtual DbSet<HistorialEstadoTipo> HistorialEstadoTipo { get; set; }
    public virtual DbSet<Horario> Horario { get; set; }
    public virtual DbSet<InversionDescuento> InversionDescuento { get; set; }
    public virtual DbSet<Ocurrencia> Ocurrencia { get; set; }
    public virtual DbSet<Tipo> Tipo { get; set; }
    public virtual DbSet<Inversion> Inversion { get; set; }
    public virtual DbSet<MetodoPago> MetodoPago { get; set; }
    public virtual DbSet<MetodoPagoProducto> MetodoPagoProducto { get; set; }
    public virtual DbSet<VentaCruzada> VentaCruzada { get; set; }
    public virtual DbSet<ProductoCertificado> ProductoCertificado { get; set; }
    public virtual DbSet<PotencialCliente> PotencialCliente { get; set; }
    public virtual DbSet<EstadoTransicion> EstadoTransicion { get; set; }   
    public virtual DbSet<CobranzaCuota> CobranzaCuota { get; set; }
    public virtual DbSet<CobranzaPago> CobranzaPago { get; set; }
    public virtual DbSet<CobranzaPagoAplicacion> CobranzaPagoAplicacion { get; set; }
    public virtual DbSet<CobranzaPlan> CobranzaPlan { get; set; }
    public virtual DbSet<Rol> Rol { get; set; }
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
            entity.HasOne(u => u.Rol)
                  .WithMany(r => r.Usuarios)
                  .HasForeignKey(u => u.IdRol)
                  .OnDelete(DeleteBehavior.Restrict);
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

            entity.Property(e => e.Estado)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.IdPais)
                .HasColumnName("IdPais")
                .IsRequired(false);

            entity.Property(e => e.IdUsuario)
                .HasColumnName("IdUsuario")
                .IsRequired(false);

            entity.HasOne(p => p.Usuario)
                .WithMany()                     
                .HasForeignKey(p => p.IdUsuario)
                .HasConstraintName("FK_Persona_Usuario")
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(p => p.Pais)
                .WithMany(pa => pa.Personas)
                .HasForeignKey(p => p.IdPais)
                .HasConstraintName("FK_Persona_Pais")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.IdPais).HasDatabaseName("IX_Persona_IdPais");

            entity.HasIndex(e => e.IdUsuario).HasDatabaseName("IX_Persona_IdUsuario");
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

            entity.Property(e => e.Estado)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

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

            entity.Property(e => e.IdTipo)
              .HasColumnName("IdTipo")
              .IsRequired();

            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired(false);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .IsRequired(false);

            entity.Property(e => e.EstadoControl)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.HasOne(h => h.Tipo)
              .WithMany(e => e.Estados)
              .HasForeignKey(h => h.IdTipo)
              .HasConstraintName("FK_Estado_Tipo")
              .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración Oportunidad
        modelBuilder.Entity<Oportunidad>(entity =>
        {
            entity.ToTable("Oportunidad", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdPotencialCliente)
                  .HasColumnName("IdPotencialCliente")
                  .IsRequired();

            entity.Property(e => e.IdProducto)
                  .HasColumnName("IdProducto")
                  .IsRequired(false);

            entity.Property(e => e.CodigoLanzamiento)
                  .HasMaxLength(255)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Origen)
                  .HasColumnName("Origen")
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relaciones
            entity.HasOne(o => o.PotencialCliente)
                  .WithMany(p => p.Oportunidades)
                  .HasForeignKey(o => o.IdPotencialCliente)
                  .HasConstraintName("FK_Oportunidad_Persona")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(o => o.Producto)
                  .WithMany(p => p.Oportunidades)
                  .HasForeignKey(o => o.IdProducto)
                  .HasConstraintName("FK_Oportunidad_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(o => o.IdPotencialCliente).HasDatabaseName("IX_Oportunidad_IdPersona");
            entity.HasIndex(o => o.IdProducto).HasDatabaseName("IX_Oportunidad_IdProducto");
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
                .HasColumnName("Estado")
                .HasDefaultValue(true);

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
            entity.Property(e => e.IdEstado).HasColumnName("IdEstado").IsRequired(false);
            entity.Property(e => e.IdOcurrencia).HasColumnName("IdOcurrencia").IsRequired(false);


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

            entity.HasOne(h => h.EstadoReferencia)
                  .WithMany(e => e.HistorialEstado)
                  .HasForeignKey(h => h.IdEstado)
                  .HasConstraintName("FK_HistorialEstado_Estado")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.Ocurrencia)
                  .WithMany()
                  .HasForeignKey(h => h.IdOcurrencia)
                  .HasConstraintName("FK_HistorialEstado_Ocurrencia")
                  .OnDelete(DeleteBehavior.Restrict);

            // índices
            entity.HasIndex(h => h.IdOportunidad).HasDatabaseName("IX_HistorialEstado_IdOportunidad");
            entity.HasIndex(h => h.IdAsesor).HasDatabaseName("IX_HistorialEstado_IdAsesor");
            entity.HasIndex(h => h.IdEstado).HasDatabaseName("IX_HistorialEstado_IdEstado");
            entity.HasIndex(h => h.IdOcurrencia).HasDatabaseName("IX_HistorialEstado_IdOcurrencia");

        });

        // Configuración HistorialInteraccion
        modelBuilder.Entity<HistorialInteraccion>(entity =>
        {
            entity.ToTable("HistorialInteraccion", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Detalle)
                  .HasMaxLength(500)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.IdTipo)
                  .HasColumnName("IdTipo")
                  .IsRequired();

            entity.Property(e => e.Celular)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.FechaRecordatorio)
                  .HasColumnType("datetime")
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdOportunidad)
                  .HasColumnName("IdOportunidad")
                  .IsRequired();

            entity.Property(e => e.IdMigracion).IsRequired(false);

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

            entity.HasOne(h => h.Oportunidad)
                  .WithMany(o => o.HistorialInteracciones)
                  .HasForeignKey(h => h.IdOportunidad)
                  .HasConstraintName("FK_HistorialInteraccion_Oportunidad")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(h => h.Tipo)
                  .WithMany(t => t.HistorialInteracciones)
                  .HasForeignKey(h => h.IdTipo)
                  .HasConstraintName("FK_HistorialInteraccion_Tipo")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(e => e.IdOportunidad).HasDatabaseName("IX_HistorialInteraccion_IdOportunidad");
            entity.HasIndex(e => e.IdTipo).HasDatabaseName("IX_HistorialInteraccion_IdTipo");
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
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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

            entity.HasMany(p => p.Personas)
                    .WithOne(per => per.Pais)
                    .HasForeignKey(per => per.IdPais)
                    .HasConstraintName("FK_Pais_Persona")
                    .OnDelete(DeleteBehavior.Restrict);

            // índice opcional si quieres buscar por Nombre
            entity.HasIndex(e => e.Nombre).HasDatabaseName("IX_Pais_Nombre");
        });

        // Configuración Beneficio
        modelBuilder.Entity<Beneficio>(entity =>
        {
            entity.ToTable("Beneficio", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Descripcion)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Descripcion");

            entity.Property(e => e.Orden);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

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

            entity.Property(e => e.IdProducto).HasColumnName("IdProducto").IsRequired();

            // Relaciones
            entity.HasOne(b => b.Producto)
                  .WithMany(p => p.Beneficios)
                  .HasForeignKey(b => b.IdProducto)
                  .HasConstraintName("FK_Beneficio_Producto")
                  .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(b => b.IdProducto).HasDatabaseName("IX_Beneficio_IdProducto");
        });

        // Configuración Certificado
        modelBuilder.Entity<Certificado>(entity =>
        {
            entity.ToTable("Certificado", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Codigo)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado");

            entity.Property(e => e.IdMigracion);

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

            entity.HasIndex(c => c.Codigo).HasDatabaseName("IX_Certificado_Codigo");
        });

        // Configuración Cobranza
        modelBuilder.Entity<Cobranza>(entity =>
        {
            entity.ToTable("Cobranza", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdHistorialEstado).HasColumnName("IdHistorialEstado").IsRequired(false);
            entity.Property(e => e.IdInversion).HasColumnName("IdInversion").IsRequired(false);
            entity.Property(e => e.IdProducto).HasColumnName("IdProducto").IsRequired(false);

            entity.Property(e => e.MontoTotal)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0.00m)
                .IsRequired();

            entity.Property(e => e.NumeroCuotas);

            entity.Property(e => e.MontoPorCuota)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            entity.Property(e => e.MontoPagado)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            entity.Property(e => e.MontoRestante)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

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

            // Relaciones (según DDL: ON DELETE NO ACTION / default -> Restrict)
            entity.HasOne(c => c.HistorialEstado)
                  .WithMany(h => h.Cobranzas)
                  .HasForeignKey(c => c.IdHistorialEstado)
                  .HasConstraintName("FK_Cobranza_HistorialEstado")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Inversion)
                  .WithMany(i => i.Cobranzas)
                  .HasForeignKey(c => c.IdInversion)
                  .HasConstraintName("FK_Cobranza_Inversion")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Producto)
                  .WithMany(p => p.Cobranzas)
                  .HasForeignKey(c => c.IdProducto)
                  .HasConstraintName("FK_Cobranza_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(c => c.IdHistorialEstado).HasDatabaseName("IX_Cobranza_IdHistorialEstado");
            entity.HasIndex(c => c.IdInversion).HasDatabaseName("IX_Cobranza_IdInversion");
            entity.HasIndex(c => c.IdProducto).HasDatabaseName("IX_Cobranza_IdProducto");
        });

        // Configuración Convertido
        modelBuilder.Entity<Convertido>(entity =>
        {
            entity.ToTable("Convertido", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.PagoCompleto)
                  .HasColumnName("PagoCompleto")
                  .HasDefaultValue(false);

            entity.Property(e => e.MontoPagado)
                  .HasColumnType("decimal(18,2)");

            entity.Property(e => e.FechaPago)
                  .HasColumnType("datetime");

            entity.Property(e => e.Moneda)
                  .HasMaxLength(10)
                  .IsUnicode(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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

            entity.Property(e => e.IdHistorialEstado).HasColumnName("IdHistorialEstado").IsRequired(false);
            entity.Property(e => e.IdInversion).HasColumnName("IdInversion").IsRequired(false);
            entity.Property(e => e.IdProducto).HasColumnName("IdProducto").IsRequired(false);

            // Relaciones
            entity.HasOne(e => e.HistorialEstado)
                  .WithMany(h => h.Convertidos)
                  .HasForeignKey(e => e.IdHistorialEstado)
                  .HasConstraintName("FK_Convertido_HistorialEstado")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Inversion)
                  .WithMany(i => i.Convertidos)
                  .HasForeignKey(e => e.IdInversion)
                  .HasConstraintName("FK_Convertido_Inversion")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Convertidos)
                  .HasForeignKey(e => e.IdProducto)
                  .HasConstraintName("FK_Convertido_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(e => e.IdHistorialEstado).HasDatabaseName("IX_Convertido_IdHistorialEstado");
            entity.HasIndex(e => e.IdInversion).HasDatabaseName("IX_Convertido_IdInversion");
            entity.HasIndex(e => e.IdProducto).HasDatabaseName("IX_Convertido_IdProducto");
        });

        // Configuración Corporativo
        modelBuilder.Entity<Corporativo>(entity =>
        {
            entity.ToTable("Corporativo", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Cantidad);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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

            entity.Property(e => e.IdHistorialEstado).HasColumnName("IdHistorialEstado").IsRequired(false);
            entity.Property(e => e.IdProducto).HasColumnName("IdProducto").IsRequired(false);
            entity.Property(e => e.IdFase).HasColumnName("IdFase").IsRequired(false);
            entity.Property(e => e.IdEmpresa).HasColumnName("IdEmpresa").IsRequired(false);

            // Relaciones
            entity.HasOne(e => e.HistorialEstado)
                  .WithMany(h => h.Corporativos)
                  .HasForeignKey(e => e.IdHistorialEstado)
                  .HasConstraintName("FK_Corporativo_HistorialEstado")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Producto)
                  .WithMany(p => p.Corporativos)
                  .HasForeignKey(e => e.IdProducto)
                  .HasConstraintName("FK_Corporativo_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(e => e.IdHistorialEstado).HasDatabaseName("IX_Corporativo_IdHistorialEstado");
            entity.HasIndex(e => e.IdProducto).HasDatabaseName("IX_Corporativo_IdProducto");
        });

        // Configuración Docente
        modelBuilder.Entity<Docente>(entity =>
        {
            entity.ToTable("Docente", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.TituloProfesional)
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.Especialidad)
                  .HasMaxLength(200)
                  .IsUnicode(false);

            entity.Property(e => e.Logros)
                  .HasColumnType("varchar(max)")
                  .IsUnicode(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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

            entity.Property(e => e.IdPersona)
                  .HasColumnName("IdPersona")
                  .IsRequired();

            // Relación 1:1 con Persona (FK IdPersona)
            entity.HasOne(d => d.Persona)
                  .WithOne(p => p.Docente)
                  .HasForeignKey<Docente>(d => d.IdPersona)
                  .HasConstraintName("FK_Docente_Persona")
                  .OnDelete(DeleteBehavior.Restrict);

            // Indice para asegurar 1:1
            entity.HasIndex(d => d.IdPersona)
                  .IsUnique()
                  .HasDatabaseName("UX_Docente_IdPersona");
        });

        // Configuración HistorialEstadoTipo
        modelBuilder.Entity<HistorialEstadoTipo>(entity =>
        {
            entity.ToTable("HistorialEstadoTipo", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdHistorialEstado)
                  .HasColumnName("IdHistorialEstado")
                  .IsRequired();

            entity.Property(e => e.IdTipo)
                  .HasColumnName("IdTipo")
                  .IsRequired();

            entity.Property(e => e.FechaCreacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.FechaModificacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioModificacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasDefaultValue("SYSTEM");

            // Relaciones
            entity.HasOne(ht => ht.HistorialEstado)
                .WithMany(h => h.HistorialEstadoTipos)
                .HasForeignKey(ht => ht.IdHistorialEstado)
                .HasConstraintName("FK_HistorialEstadoTipo_HistorialEstado")
                .OnDelete(DeleteBehavior.Cascade);

            // FK hacia Tipo (ON DELETE CASCADE en DDL)
            entity.HasOne(ht => ht.Tipo)
                .WithMany(t => t.HistorialEstadoTipos) // Tipo sí tenía la colección según tus modelos previos
                .HasForeignKey(ht => ht.IdTipo)
                .HasConstraintName("FK_HistorialEstadoTipo_Tipo")
                .OnDelete(DeleteBehavior.Cascade);

            // Índices / unicidad
            entity.HasIndex(e => new { e.IdHistorialEstado, e.IdTipo })
                  .IsUnique()
                  .HasDatabaseName("UX_HistorialEstadoTipo_Unico");
        });

        // Configuración Horario
        modelBuilder.Entity<Horario>(entity =>
        {
            entity.ToTable("Horario", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdProducto)
                  .HasColumnName("IdProducto")
                  .IsRequired();

            entity.Property(e => e.Dia)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.HoraInicio)
                  .HasColumnType("time");

            entity.Property(e => e.HoraFin)
                  .HasColumnType("time");

            entity.Property(e => e.Detalle)
                  .HasMaxLength(255)
                  .IsUnicode(false);

            entity.Property(e => e.Orden);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

            entity.Property(e => e.FechaCreacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.FechaModificacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioModificacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            // Relación con Producto (ON DELETE CASCADE según DDL)
            entity.HasOne(h => h.Producto)
                  .WithMany() // si Producto tiene colección (eg. Product.Horarios) cámbialo por .WithMany(p => p.Horarios)
                  .HasForeignKey(h => h.IdProducto)
                  .HasConstraintName("FK_Horario_Producto")
                  .OnDelete(DeleteBehavior.Cascade);

            // Índice para consultas por producto
            entity.HasIndex(h => h.IdProducto).HasDatabaseName("IX_Horario_IdProducto");
        });

        // Configuración InversionDescuento
        modelBuilder.Entity<InversionDescuento>(entity =>
        {
            entity.ToTable("InversionDescuento", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdInversion)
                  .HasColumnName("IdInversion")
                  .IsRequired();

            entity.Property(e => e.Nombre)
                  .HasMaxLength(200)
                  .IsUnicode(false);

            // Porcentaje decimal(6,2) según DDL
            entity.Property(e => e.Porcentaje)
                  .HasColumnType("decimal(6,2)");

            // Monto decimal(18,2)
            entity.Property(e => e.Monto)
                  .HasColumnType("decimal(18,2)");

            entity.Property(e => e.FechaInicio)
                  .HasColumnType("datetime");

            entity.Property(e => e.FechaFin)
                  .HasColumnType("datetime");

            entity.Property(e => e.Activo)
                  .HasDefaultValue(true);

            entity.Property(e => e.Estado)
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

            entity.Property(e => e.FechaCreacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.FechaModificacion)
                  .HasColumnType("datetime")
                  .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioModificacion)
                  .HasMaxLength(50)
                  .IsUnicode(false)
                  .HasDefaultValue("SYSTEM");

            entity.Property(e => e.IdTipo)
                  .HasColumnName("IdTipo");

            entity.HasOne(d => d.Inversion)
                  .WithMany(i => i.Descuentos)
                  .HasForeignKey(d => d.IdInversion)
                  .HasConstraintName("FK_InversionDescuento_Inversion")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Tipo)
                  .WithMany(t => t.InversionDescuentos)
                  .HasForeignKey(d => d.IdTipo)
                  .HasConstraintName("FK_InversionDescuento_Tipo")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(d => d.IdInversion)
                  .HasDatabaseName("UX_InversionDescuento_UnicoActivo")
                  .IsUnique(false)
                  .HasFilter("[Activo] = 1 AND [Estado] = 1");
        });

        // Configuración Ocurrencia
        modelBuilder.Entity<Ocurrencia>(entity =>
        {
            entity.ToTable("Ocurrencia", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .HasMaxLength(255)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Descripcion)
                  .HasMaxLength(255)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.IdEstado)
                  .HasColumnName("IdEstado")
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            entity.HasOne(o => o.EstadoReferencia)
                  .WithMany(e => e.Ocurrencias)
                  .HasForeignKey(o => o.IdEstado)
                  .HasConstraintName("FK_Ocurrencia_Estado")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(o => o.IdEstado).HasDatabaseName("IX_Ocurrencia_IdEstado");
        });

        // Configuración Inversion
        modelBuilder.Entity<Inversion>(entity =>
        {
            entity.ToTable("Inversion", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdProducto)
                  .HasColumnName("IdProducto")
                  .IsRequired();

            entity.Property(e => e.IdOportunidad)
                  .HasColumnName("IdOportunidad")
                  .IsRequired();

            entity.Property(e => e.CostoTotal)
                  .HasColumnType("decimal(18,2)")
                  .HasDefaultValue(0.00m);

            entity.Property(e => e.Moneda)
                  .HasMaxLength(10)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.DescuentoPorcentaje)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired(false);

            entity.Property(e => e.DescuentoMonto)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired(false);

            entity.Property(e => e.CostoOfrecido)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relaciones
            entity.HasOne(i => i.Producto)
                  .WithMany(p => p.Inversiones)
                  .HasForeignKey(i => i.IdProducto)
                  .HasConstraintName("FK_Inversion_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(i => i.Oportunidad)
                  .WithMany(o => o.Inversion)
                  .HasForeignKey(i => i.IdOportunidad)
                  .HasConstraintName("FK_Inversion_Oportunidad")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(i => i.IdProducto).HasDatabaseName("IX_Inversion_IdProducto");
            entity.HasIndex(i => i.IdOportunidad).HasDatabaseName("IX_Inversion_IdOportunidad");
        });

        // Configuración MetodoPago
        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.ToTable("MetodoPago", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .HasMaxLength(200)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.Activo)
                  .HasColumnName("Activo")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relaciones: MetodoPago -> MetodoPagoProducto (1:N)
            entity.HasMany(m => m.MetodoPagoProductos)
                  .WithOne(mpp => mpp.MetodoPago)
                  .HasForeignKey(mpp => mpp.IdMetodoPago)
                  .HasConstraintName("FK_MetodoPago_MetodoPagoProducto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices opcionales (añade si quieres consultas por Activo rápido)
            entity.HasIndex(e => e.Activo).HasDatabaseName("IX_MetodoPago_Activo");
        });

        // Configuración MetodoPagoProducto
        modelBuilder.Entity<MetodoPagoProducto>(entity =>
        {
            entity.ToTable("MetodoPagoProducto", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdProducto)
                  .HasColumnName("IdProducto")
                  .IsRequired();

            entity.Property(e => e.IdMetodoPago)
                  .HasColumnName("IdMetodoPago")
                  .IsRequired();

            entity.Property(e => e.Activo)
                  .HasColumnName("Activo")
                  .HasDefaultValue(true);

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
                  .IsUnicode(false)
                  .HasDefaultValue("SYSTEM");

            // Relaciones
            entity.HasOne(mpp => mpp.Producto)
                  .WithMany(p => p.MetodoPagoProductos)
                  .HasForeignKey(mpp => mpp.IdProducto)
                  .HasConstraintName("FK_MetodoPagoProducto_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(mpp => mpp.MetodoPago)
                  .WithMany(mp => mp.MetodoPagoProductos)
                  .HasForeignKey(mpp => mpp.IdMetodoPago)
                  .HasConstraintName("FK_MPP_MetodoPago")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices para consultas rápidas por FK
            entity.HasIndex(e => e.IdProducto).HasDatabaseName("IX_MetodoPagoProducto_IdProducto");
            entity.HasIndex(e => e.IdMetodoPago).HasDatabaseName("IX_MetodoPagoProducto_IdMetodoPago");
        });

        // Configuración ProductoCertificado
        modelBuilder.Entity<ProductoCertificado>(entity =>
        {
            entity.ToTable("ProductoCertificado", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdProducto)
                  .HasColumnName("IdProducto")
                  .IsRequired();

            entity.Property(e => e.IdCertificado)
                  .HasColumnName("IdCertificado")
                  .IsRequired();

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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
                  .IsUnicode(false)
                  .HasDefaultValue("SYSTEM");

            // Relaciones
            entity.HasOne(pc => pc.Producto)
                  .WithMany(p => p.ProductoCertificados)
                  .HasForeignKey(pc => pc.IdProducto)
                  .HasConstraintName("FK_ProductoCertificado_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(pc => pc.Certificado)
                  .WithMany(c => c.ProductoCertificados)
                  .HasForeignKey(pc => pc.IdCertificado)
                  .HasConstraintName("FK_ProductoCertificado_Certificado")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(e => e.IdProducto).HasDatabaseName("IX_ProductoCertificado_IdProducto");
            entity.HasIndex(e => e.IdCertificado).HasDatabaseName("IX_ProductoCertificado_IdCertificado");
        });

        // Configuración VentaCruzada
        modelBuilder.Entity<VentaCruzada>(entity =>
        {
            entity.ToTable("VentaCruzada", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdHistorialEstado)
                  .HasColumnName("IdHistorialEstado")
                  .IsRequired(false);

            entity.Property(e => e.IdProductoOrigen)
                  .HasColumnName("IdProductoOrigen")
                  .IsRequired(false);

            entity.Property(e => e.IdProductoDestino)
                  .HasColumnName("IdProductoDestino")
                  .IsRequired(false);

            entity.Property(e => e.IdFase)
                  .HasColumnName("IdFase")
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

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
                  .IsUnicode(false)
                  .HasDefaultValue("SYSTEM");

            // Relaciones
            entity.HasOne(v => v.HistorialEstado)
                  .WithMany(h => h.VentaCruzadas)
                  .HasForeignKey(v => v.IdHistorialEstado)
                  .HasConstraintName("FK_VC_HistorialEstado")
                  .OnDelete(DeleteBehavior.Restrict);

            // Producto origen
            entity.HasOne(v => v.ProductoOrigen)
                  .WithMany(p => p.VentaCruzadaOrigen)
                  .HasForeignKey(v => v.IdProductoOrigen)
                  .HasConstraintName("FK_VC_ProductoOrigen")
                  .OnDelete(DeleteBehavior.Restrict);

            // Producto destino
            entity.HasOne(v => v.ProductoDestino)
                  .WithMany(p => p.VentaCruzadaDestino)
                  .HasForeignKey(v => v.IdProductoDestino)
                  .HasConstraintName("FK_VC_ProductoDestino")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(v => v.IdHistorialEstado).HasDatabaseName("IX_VentaCruzada_IdHistorialEstado");
            entity.HasIndex(v => v.IdProductoOrigen).HasDatabaseName("IX_VentaCruzada_IdProductoOrigen");
            entity.HasIndex(v => v.IdProductoDestino).HasDatabaseName("IX_VentaCruzada_IdProductoDestino");
        });

        // Configuración Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.ToTable("Producto", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .HasMaxLength(500)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.CodigoLanzamiento)
                  .HasMaxLength(255)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.FechaInicio)
                  .HasColumnType("datetime")
                  .IsRequired(false);

            entity.Property(e => e.FechaPresentacion)
                  .HasColumnType("datetime")
                  .IsRequired(false);

            entity.Property(e => e.DatosImportantes)
                  .HasColumnType("varchar(max)")
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relaciones

            // Horarios
            entity.HasMany(p => p.Horarios)
                  .WithOne(h => h.Producto)
                  .HasForeignKey(h => h.IdProducto)
                  .HasConstraintName("FK_Horario_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Inversiones
            entity.HasMany(p => p.Inversiones)
                  .WithOne(i => i.Producto)
                  .HasForeignKey(i => i.IdProducto)
                  .HasConstraintName("FK_Inversion_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // MetodoPagoProducto
            entity.HasMany(p => p.MetodoPagoProductos)
                  .WithOne(mpp => mpp.Producto)
                  .HasForeignKey(mpp => mpp.IdProducto)
                  .HasConstraintName("FK_MPP_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // ProductoCertificado
            entity.HasMany(p => p.ProductoCertificados)
                  .WithOne(pc => pc.Producto)
                  .HasForeignKey(pc => pc.IdProducto)
                  .HasConstraintName("FK_ProductoCertificado_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Beneficios
            entity.HasMany(p => p.Beneficios)
                  .WithOne(b => b.Producto)
                  .HasForeignKey(b => b.IdProducto)
                  .HasConstraintName("FK_Beneficio_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Cobranzas
            entity.HasMany(p => p.Cobranzas)
                  .WithOne(c => c.Producto)
                  .HasForeignKey(c => c.IdProducto)
                  .HasConstraintName("FK_Cobranza_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // Convertidos
            entity.HasMany(p => p.Convertidos)
                  .WithOne(conv => conv.Producto)
                  .HasForeignKey(conv => conv.IdProducto)
                  .HasConstraintName("FK_Convertido_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

            // VentaCruzada origen
            entity.HasMany(p => p.VentaCruzadaOrigen)
                  .WithOne(vc => vc.ProductoOrigen)
                  .HasForeignKey(vc => vc.IdProductoOrigen)
                  .HasConstraintName("FK_VC_ProductoOrigen")
                  .OnDelete(DeleteBehavior.Restrict);

            // VentaCruzada destino
            entity.HasMany(p => p.VentaCruzadaDestino)
                  .WithOne(vc => vc.ProductoDestino)
                  .HasForeignKey(vc => vc.IdProductoDestino)
                  .HasConstraintName("FK_VC_ProductoDestino")
                  .OnDelete(DeleteBehavior.Restrict);

            // Corporativos
            entity.HasMany(p => p.Corporativos)
                  .WithOne(c => c.Producto)
                  .HasForeignKey(c => c.IdProducto)
                  .HasConstraintName("FK_Corporativo_Producto")
                  .OnDelete(DeleteBehavior.Restrict);

        });

        // Configuración Tipo
        modelBuilder.Entity<Tipo>(entity =>
        {
            entity.ToTable("Tipo", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nombre)
                  .HasMaxLength(200)
                  .IsUnicode(false)
                  .IsRequired();

            entity.Property(e => e.Descripcion)
                  .HasMaxLength(500)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Categoria)
                  .HasMaxLength(100)
                  .IsUnicode(false)
                  .IsRequired(false);

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relaciones (según DDL)
            entity.HasMany(t => t.Estados)
                  .WithOne(e => e.Tipo)
                  .HasForeignKey(e => e.IdTipo)
                  .HasConstraintName("FK_Estado_Tipo")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.HistorialEstadoTipos)
                  .WithOne(ht => ht.Tipo)
                  .HasForeignKey(ht => ht.IdTipo)
                  .HasConstraintName("FK_HistorialEstadoTipo_Tipo")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(t => t.HistorialInteracciones)
                  .WithOne(hi => hi.Tipo)
                  .HasForeignKey(hi => hi.IdTipo)
                  .HasConstraintName("FK_HistorialInteraccion_Tipo")
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(t => t.InversionDescuentos)
                  .WithOne(id => id.Tipo)
                  .HasForeignKey(id => id.IdTipo)
                  .HasConstraintName("FK_InversionDescuento_Tipo")
                  .OnDelete(DeleteBehavior.Restrict);

            // Índices opcionales
            entity.HasIndex(e => e.Nombre).HasDatabaseName("IX_Tipo_Nombre");
        });

        // Configuración PotencialCliente
        modelBuilder.Entity<PotencialCliente>(entity =>
        {
            entity.ToTable("PotencialCliente", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdPersona)
                  .HasColumnName("IdPersona")
                  .IsRequired();

            entity.Property(e => e.Desuscrito)
                  .HasColumnName("Desuscrito")
                  .IsRequired();

            entity.Property(e => e.Estado)
                  .HasColumnName("Estado")
                  .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

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

            // Relación 1:1 con Persona (FK IdPersona)
            entity.HasOne(d => d.Persona)
                  .WithOne(p => p.PotencialCliente)
                  .HasForeignKey<PotencialCliente>(d => d.IdPersona)
                  .HasConstraintName("FK_PotencialCliente_Persona");

            // Indice para asegurar 1:1
            entity.HasIndex(d => d.IdPersona)
                  .IsUnique()
                  .HasDatabaseName("UX_PotencialCliente_IdPersona");
        });
        // Configuración EstadoTransicion
        modelBuilder.Entity<EstadoTransicion>(entity =>
        {
            entity.ToTable("EstadoTransicion", schema: "adm");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.IdEstadoOrigen)
                .HasColumnName("IdEstadoOrigen");

            entity.Property(e => e.IdEstadoDestino)
                .HasColumnName("IdEstadoDestino");

            entity.Property(e => e.IdOcurrenciaOrigen)
                .HasColumnName("IdOcurrenciaOrigen");

            entity.Property(e => e.IdOcurrenciaDestino)
                .HasColumnName("IdOcurrenciaDestino");

            entity.Property(e => e.Permitido)
                .HasDefaultValue(true);

            entity.Property(e => e.Comentario)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.Estado)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

            entity.Property(e => e.IdMigracion);

            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioCreacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .IsRequired(false)
                .HasDefaultValueSql("(getdate())");

            entity.Property(e => e.UsuarioModificacion)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(e => e.EstadoOrigen)
                .WithMany()
                .HasForeignKey(e => e.IdEstadoOrigen)
                .HasConstraintName("FK_EstadoTransicion_EstadoOrigen")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.EstadoDestino)
                .WithMany()
                .HasForeignKey(e => e.IdEstadoDestino)
                .HasConstraintName("FK_EstadoTransicion_EstadoDestino")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OcurrenciaOrigen)
                .WithMany()
                .HasForeignKey(e => e.IdOcurrenciaOrigen)
                .HasConstraintName("FK_EstadoTransicion_OcurrenciaOrigen")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.OcurrenciaDestino)
                .WithMany()
                .HasForeignKey(e => e.IdOcurrenciaDestino)
                .HasConstraintName("FK_EstadoTransicion_OcurrenciaDestino")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.IdEstadoOrigen, e.IdEstadoDestino })
                .HasDatabaseName("IX_EstadoTransicion_OrigenDestino_Prioridad");
        });

        modelBuilder.Entity<CobranzaPlan>(entity =>
        {
            entity.ToTable("CobranzaPlan", "adm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaCreacion).HasColumnType("datetime").HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UsuarioCreacion).HasMaxLength(50).IsUnicode(false);
            entity.HasIndex(e => e.IdOportunidad).HasDatabaseName("IX_CobranzaPlan_IdOportunidad");
            entity.HasMany(e => e.Cuotas).WithOne(c => c.Plan).HasForeignKey(c => c.IdCobranzaPlan);
            entity.HasMany(e => e.Pagos).WithOne(p => p.Plan).HasForeignKey(p => p.IdCobranzaPlan);
        });

        modelBuilder.Entity<CobranzaCuota>(entity =>
        {
            entity.ToTable("CobranzaCuota", "adm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FechaVencimiento).HasColumnType("datetime");
            entity.Property(e => e.MontoProgramado).HasColumnType("decimal(18,2)");
            entity.Property(e => e.MontoPagado).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            entity.HasIndex(e => new { e.IdCobranzaPlan, e.Numero }).IsUnique().HasDatabaseName("UX_CobranzaCuota_Plan_Numero");
        });

        modelBuilder.Entity<CobranzaPago>(entity =>
        {
            entity.ToTable("CobranzaPago", "adm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FechaPago).HasColumnType("datetime");
            entity.Property(e => e.Monto).HasColumnType("decimal(18,2)");
            entity.HasIndex(e => e.IdCobranzaPlan).HasDatabaseName("IX_CobranzaPago_Plan");
        });

        modelBuilder.Entity<CobranzaPagoAplicacion>(entity =>
        {
            entity.ToTable("CobranzaPagoAplicacion", "adm");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.MontoAplicado).HasColumnType("decimal(18,2)");
            entity.HasOne(a => a.Pago).WithMany(p => p.Aplicaciones).HasForeignKey(a => a.IdPago);
            entity.HasOne(a => a.Cuota).WithMany(c => c.Aplicaciones).HasForeignKey(a => a.IdCuota);
            entity.HasIndex(e => e.IdCuota).HasDatabaseName("IX_CobranzaPagoAplicacion_Cuota");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable("Rol", schema: "dbo");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.NombreRol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .IsRequired();

            entity.Property(e => e.Estado)
                .HasColumnName("Estado")
                .HasDefaultValue(true);

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
    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
