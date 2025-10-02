using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OlympusContext _context;

        private UsuarioRepository _usuarioRepository;
        private ErrorLogRepository _errorLogRepository;
        private AreaRepository _areaRepository;
        private ModuloRepository _moduloRepository;
        private FormularioRepository _formularioRepository;
        private UserTokenRepository _userTokenRepository;

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

        public UnitOfWork(OlympusContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

