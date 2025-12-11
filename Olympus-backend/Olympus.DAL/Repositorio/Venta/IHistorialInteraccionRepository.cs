using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IHistorialInteraccionRepository
    {
        bool Insertar(HistorialInteraccion modelo);
        bool Actualizar(HistorialInteraccion modelo);
        bool Eliminar(int id);
        HistorialInteraccion? ObtenerPorId(int id);
        IQueryable<HistorialInteraccion> ObtenerTodos();
        IQueryable<HistorialInteraccion> Query();
    }
}
