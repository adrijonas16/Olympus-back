using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IMetodoPagoProductoRepository
    {
        bool Insertar(Modelos.Entidades.MetodoPagoProducto modelo);
        bool Actualizar(Modelos.Entidades.MetodoPagoProducto modelo);
        bool Eliminar(int id);
        Modelos.Entidades.MetodoPagoProducto? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.MetodoPagoProducto> ObtenerTodos();
        IQueryable<Modelos.Entidades.MetodoPagoProducto> Query();
    }
}
