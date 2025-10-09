using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;
using CapaDatos.Repositorio.Venta;

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
        IMotivoRepository MotivoRepository { get; }
        IControlOportunidadRepository ControlOportunidadRepository { get; }
        IOportunidadRepository OportunidadRepository { get; }
        IHistorialEstadoRepository HistorialEstadoRepository { get; }
        IHistorialInteraccionRepository HistorialInteraccionRepository { get; }

        // Guardar cambios
        Task<int> SaveChangesAsync();
    }
}
