using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
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

        public bool Insertar(Usuario modelo)
        {
            _context.Usuario.Add(modelo);
            return true;
        }

        public bool Actualizar(Usuario modelo)
        {
            _context.Usuario.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Usuario.FirstOrDefault(u => u.Id == id);
            if (ent == null) return false;

            _context.Usuario.Remove(ent);
            return true;
        }

        public Usuario? ObtenerPorId(int id)
        {
            return _context.Usuario
                    .Include(u => u.Rol)
                    .FirstOrDefault(u => u.Id == id);
        }

        public IQueryable<Usuario> ObtenerTodos()
        {
            return _context.Usuario
                    .Include(u => u.Rol)
                    .AsQueryable();
        }

        public Usuario? ObtenerPorCorreo(string correo)
        {
            return _context.Usuario
                           .Include(u => u.Rol)
                           .FirstOrDefault(u => u.Correo == correo);
        }

        public IQueryable<Usuario> ObtenerPorRol(int idRol)
        {
            return _context.Usuario
                           .Include(u => u.Rol)
                           .Where(u => u.IdRol == idRol && u.Activo)
                           .AsQueryable();
        }

    }
}
