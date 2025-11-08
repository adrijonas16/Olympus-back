using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IInversionDescuentoRepository
    {
        bool Insertar(InversionDescuento modelo);
        bool Actualizar(InversionDescuento modelo);
        bool Eliminar(int id);
        InversionDescuento? ObtenerPorId(int id);
        IQueryable<InversionDescuento> Query();
        IQueryable<InversionDescuento> ObtenerTodos();
    }
}
