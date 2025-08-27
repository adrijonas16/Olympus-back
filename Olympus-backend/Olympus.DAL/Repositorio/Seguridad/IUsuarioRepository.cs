using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{ 
    public interface IUsuarioRepository
    {
        bool Insertar(Usuario modelo);
        bool Actualizar(Usuario modelo);
        bool Eliminar(int id);
        Usuario ObtenerPorId(int id);
        Usuario ObtenerPorCorreo(string correo);
        IQueryable<Usuario> ObtenerTodos();
    }
}
