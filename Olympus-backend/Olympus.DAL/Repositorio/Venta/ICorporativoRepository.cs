using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICorporativoRepository
    {
        bool Insertar(Corporativo modelo);
        bool Actualizar(Corporativo modelo);
        bool Eliminar(int id);
        Corporativo? ObtenerPorId(int id);
        IQueryable<Corporativo> Query();
        IQueryable<Corporativo> ObtenerTodos();
    }
}
