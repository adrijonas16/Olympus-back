using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IEstadoRepository
    {
        bool Insertar(Estado modelo);
        bool Actualizar(Estado modelo);
        bool Eliminar(int id);
        Estado? ObtenerPorId(int id);
        IQueryable<Estado> ObtenerTodos();
        IQueryable<Estado> Query();
    }
}
