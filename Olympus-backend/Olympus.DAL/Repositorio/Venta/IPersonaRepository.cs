using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IPersonaRepository
    {
        bool Insertar(Persona modelo);
        bool Actualizar(Persona modelo);
        bool Eliminar(int id);
        Persona? ObtenerPorId(int id);
        IQueryable<Persona> ObtenerTodos();
        Persona? ObtenerPorIdUsuario(int idUsuario);

    }
}
