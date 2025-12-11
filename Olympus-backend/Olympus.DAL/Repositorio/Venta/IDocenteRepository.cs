using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IDocenteRepository
    {
        bool Insertar(Docente modelo);
        bool Actualizar(Docente modelo);
        bool Eliminar(int id);
        Docente? ObtenerPorId(int id);
        IQueryable<Docente> Query();
        IQueryable<Docente> ObtenerTodos();
    }
}
