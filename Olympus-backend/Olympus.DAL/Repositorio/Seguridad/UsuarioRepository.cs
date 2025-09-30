using CapaDatos.DataContext;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly OlympusContext _context;
        public UsuarioRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Actualizar(Usuario modelo)
        {
            throw new NotImplementedException();
        }

        public bool Eliminar(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insertar(Usuario modelo)
        {
            throw new NotImplementedException();
        }

        public Usuario? ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Usuario> ObtenerTodos()
        {
            throw new NotImplementedException();
        }

        public Usuario? ObtenerPorCorreo(string correo)
        {
            return _context.Usuario.FirstOrDefault(u => u.Correo == correo);
        }
    }
}
