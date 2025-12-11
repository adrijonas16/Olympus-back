using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IConvertidoRepository
    {
        bool Insertar(Convertido modelo);
        bool Actualizar(Convertido modelo);
        bool Eliminar(int id);
        Convertido? ObtenerPorId(int id);
        IQueryable<Convertido> Query();
        IQueryable<Convertido> ObtenerTodos();
    }
}
