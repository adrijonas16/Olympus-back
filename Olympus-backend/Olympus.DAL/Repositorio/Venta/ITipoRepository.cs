using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ITipoRepository
    {
        IQueryable<Modelos.Entidades.Tipo> ObtenerTodos();
        IQueryable<Modelos.Entidades.Tipo> Query();
        Modelos.Entidades.Tipo? ObtenerPorId(int id);
        bool Insertar(Modelos.Entidades.Tipo entidad);
        bool Actualizar(Modelos.Entidades.Tipo entidad);
        bool Eliminar(int id);
    }
}
