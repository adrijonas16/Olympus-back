using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OlympusContext _context;

        private UsuarioRepository _usuarioRepository;

        public IUsuarioRepository UsuarioRepository
            => _usuarioRepository ??= new UsuarioRepository(_context);

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

