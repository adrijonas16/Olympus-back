using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IMetodoPagoRepository
    {
        bool Insertar(Modelos.Entidades.MetodoPago modelo);
        bool Actualizar(Modelos.Entidades.MetodoPago modelo);
        bool Eliminar(int id);
        Modelos.Entidades.MetodoPago? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.MetodoPago> ObtenerTodos();
        IQueryable<Modelos.Entidades.MetodoPago> Query();
    }
}
