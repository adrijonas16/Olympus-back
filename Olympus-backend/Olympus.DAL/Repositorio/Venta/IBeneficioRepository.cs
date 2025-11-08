using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface IBeneficioRepository
    {
        bool Insertar(Beneficio modelo);
        bool Actualizar(Beneficio modelo);
        bool Eliminar(int id);
        Beneficio? ObtenerPorId(int id);
        IQueryable<Beneficio> Query();
        IQueryable<Beneficio> ObtenerTodos();
    }
}
