using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICobranzaRepository
    {
        bool Insertar(Cobranza modelo);
        bool Actualizar(Cobranza modelo);
        bool Eliminar(int id);
        Cobranza? ObtenerPorId(int id);
        IQueryable<Cobranza> Query();
        IQueryable<Cobranza> ObtenerTodos();
    }
}
