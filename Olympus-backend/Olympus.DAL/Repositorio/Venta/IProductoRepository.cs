using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IProductoRepository
    {
        IQueryable<Producto> Query();
        IQueryable<Producto> ObtenerTodos();
        Modelos.Entidades.Producto? ObtenerPorId(int id);
        bool Insertar(Modelos.Entidades.Producto entidad);
        bool Actualizar(Modelos.Entidades.Producto entidad);
        bool Eliminar(int id);
    }
}
