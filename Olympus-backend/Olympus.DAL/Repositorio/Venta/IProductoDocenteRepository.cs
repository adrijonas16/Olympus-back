using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IProductoDocenteRepository
    {
        bool Insertar(Modelos.Entidades.ProductoDocente modelo);
        bool Actualizar(Modelos.Entidades.ProductoDocente modelo);
        bool Eliminar(int id);
        Modelos.Entidades.ProductoDocente? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.ProductoDocente> ObtenerTodos();
        IQueryable<Modelos.Entidades.ProductoDocente> Query();
    }
}
