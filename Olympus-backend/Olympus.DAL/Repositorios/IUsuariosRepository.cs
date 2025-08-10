
using Modelos;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos.Repositorios
{ 
    public interface IUsuariosRepository
    {
        Task<bool> Insertar(Usuario modelo);
        Task<bool> Actualizar(Usuario modelo);
        Task<bool> Eliminar(int id);
        Task<Usuario?> ObtenerPorId(int id);
        Task<Usuario?> ObtenerPorCorreo(string correo);
        Task<IQueryable<Usuario>> ObtenerTodos();
    }
}
