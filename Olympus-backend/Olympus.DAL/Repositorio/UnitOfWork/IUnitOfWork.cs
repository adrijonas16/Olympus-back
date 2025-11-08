using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;
using CapaDatos.Repositorio.Venta;
using Microsoft.EntityFrameworkCore.Storage;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public interface IUnitOfWork
    {
        // Aquí defines tus repositorios
        IUsuarioRepository UsuarioRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IAreaRepository AreaRepository { get; }
        IModuloRepository ModuloRepository { get; }
        IFormularioRepository FormularioRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IPersonaRepository PersonaRepository { get; }
        IEstadoRepository EstadoRepository { get; }
        IAsesorRepository AsesorRepository { get; }
        IControlOportunidadRepository ControlOportunidadRepository { get; }
        IOportunidadRepository OportunidadRepository { get; }
        IHistorialEstadoRepository HistorialEstadoRepository { get; }
        IHistorialInteraccionRepository HistorialInteraccionRepository { get; }
        IPaisRepository PaisRepository { get; }
        ILanzamientoRepository LanzamientoRepository { get; }
        IBeneficioRepository BeneficioRepository { get; }
        IProductoRepository ProductoRepository { get; }
        ITipoRepository TipoRepository { get; }
        ICertificadoRepository CertificadoRepository { get; }
        ICobranzaRepository CobranzaRepository { get; }
        IConvertidoRepository ConvertidoRepository { get; }
        ICorporativoRepository CorporativoRepository { get; }
        IDocenteRepository DocenteRepository { get; }
        IHistorialEstadoTipoRepository HistorialEstadoTipoRepository { get; }
        IHorarioRepository HorarioRepository { get; }
        IInversionDescuentoRepository InversionDescuentoRepository { get; }
        IOcurrenciaRepository OcurrenciaRepository { get; }
        IInversionRepository InversionRepository { get; }
        IMetodoPagoRepository MetodoPagoRepository { get; }
        IMetodoPagoProductoRepository MetodoPagoProductoRepository { get; }
        IProductoDocenteRepository ProductoDocenteRepository { get; }
        IProductoCertificadoRepository ProductoCertificadoRepository { get; }
        IVentaCruzadaRepository VentaCruzadaRepository { get; }

        // Guardar cambios
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
