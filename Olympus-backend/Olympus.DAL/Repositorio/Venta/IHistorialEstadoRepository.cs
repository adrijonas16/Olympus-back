using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IHistorialEstadoRepository
    {
        bool Insertar(HistorialEstado modelo);
        bool Actualizar(HistorialEstado modelo);
        bool Eliminar(int id);
        HistorialEstado? ObtenerPorId(int id);
        IQueryable<HistorialEstado> ObtenerTodos();
    }
}
