using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;
using CapaDatos.Repositorio.Venta;
using Microsoft.EntityFrameworkCore.Storage;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OlympusContext _context;

        private IDbContextTransaction? _transaction;

        private UsuarioRepository _usuarioRepository;
        private ErrorLogRepository _errorLogRepository;
        private AreaRepository _areaRepository;
        private ModuloRepository _moduloRepository;
        private FormularioRepository _formularioRepository;
        private UserTokenRepository _userTokenRepository;
        private PersonaRepository _personaRepository;
        private AsesorRepository _asesorRepository;
        private EstadoRepository _estadoRepository;
        private MotivoRepository _motivoRepository;
        private OportunidadRepository _oportunidadRepository;
        private ControlOportunidadRepository _controlOportunidadRepository;
        private HistorialEstadoRepository _historialEstadoRepository;
        private HistorialInteraccionRepository _historialInteraccionRepository;
        private PaisRepository _paisRepository;
        private LanzamientoRepository _lanzamientoRepository;

        public IUsuarioRepository UsuarioRepository
            => _usuarioRepository ??= new UsuarioRepository(_context);
        public IErrorLogRepository ErrorLogRepository
            => _errorLogRepository ??= new ErrorLogRepository(_context);
        public IAreaRepository AreaRepository
            => _areaRepository ??= new AreaRepository(_context);
        public IModuloRepository ModuloRepository
            => _moduloRepository ??= new ModuloRepository(_context);
        public IFormularioRepository FormularioRepository
            => _formularioRepository ??= new FormularioRepository(_context);
        public IUserTokenRepository UserTokenRepository
            => _userTokenRepository ??= new UserTokenRepository(_context);
        public IPersonaRepository PersonaRepository
            => _personaRepository ??= new PersonaRepository(_context);
        public IAsesorRepository AsesorRepository
            => _asesorRepository ??= new AsesorRepository(_context);
        public IEstadoRepository EstadoRepository
         => _estadoRepository ??= new EstadoRepository(_context);
        public IMotivoRepository MotivoRepository
            => _motivoRepository ??= new MotivoRepository(_context);
        public IOportunidadRepository OportunidadRepository
            => _oportunidadRepository ??= new OportunidadRepository(_context);
        public IControlOportunidadRepository ControlOportunidadRepository
            => _controlOportunidadRepository ??= new ControlOportunidadRepository(_context);
        public IHistorialEstadoRepository HistorialEstadoRepository
            => _historialEstadoRepository ??= new HistorialEstadoRepository(_context);
        public IHistorialInteraccionRepository HistorialInteraccionRepository
            => _historialInteraccionRepository ??= new HistorialInteraccionRepository(_context);
        public IPaisRepository PaisRepository
            => _paisRepository ??= new PaisRepository(_context);
        public ILanzamientoRepository LanzamientoRepository
            => _lanzamientoRepository ??= new LanzamientoRepository(_context);

        public UnitOfWork(OlympusContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction != null)
                return _transaction;

            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null) return;

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

