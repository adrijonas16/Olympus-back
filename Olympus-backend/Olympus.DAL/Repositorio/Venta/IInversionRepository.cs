using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IInversionRepository
    {
        bool Insertar(Modelos.Entidades.Inversion modelo);
        bool Actualizar(Modelos.Entidades.Inversion modelo);
        bool Eliminar(int id);
        Modelos.Entidades.Inversion? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.Inversion> ObtenerTodos();
        IQueryable<Modelos.Entidades.Inversion> Query();
    }
}
