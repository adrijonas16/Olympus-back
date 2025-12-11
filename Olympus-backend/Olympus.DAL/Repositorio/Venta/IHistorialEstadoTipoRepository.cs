using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IHistorialEstadoTipoRepository
    {
        bool Insertar(HistorialEstadoTipo modelo);
        bool Actualizar(HistorialEstadoTipo modelo);
        bool Eliminar(int id);
        HistorialEstadoTipo? ObtenerPorId(int id);
        IQueryable<HistorialEstadoTipo> Query();
        IQueryable<HistorialEstadoTipo> ObtenerTodos();
    }
}
