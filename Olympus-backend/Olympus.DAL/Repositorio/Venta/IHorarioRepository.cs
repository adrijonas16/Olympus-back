using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IHorarioRepository
    {
        bool Insertar(Horario modelo);
        bool Actualizar(Horario modelo);
        bool Eliminar(int id);
        Horario? ObtenerPorId(int id);
        IQueryable<Horario> Query();
        IQueryable<Horario> ObtenerTodos();
    }
}
