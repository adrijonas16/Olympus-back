using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos;

namespace CapaDatos.Repositorios
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly OlympusContext _context;
        public UsuariosRepository(OlympusContext context)
        {
            _context = context;
        }
        public Task<bool> Actualizar(Usuario modelo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insertar(Usuario modelo)
        {
            throw new NotImplementedException();
        }

        public Task<Usuario?> ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Usuario>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario?> ObtenerPorCorreo(string correo)
        {
            return await _context.Usuarios
                                 .FirstOrDefaultAsync(u => u.Correo == correo);
        }
    }
}
