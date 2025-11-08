using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IVentaCruzadaRepository
    {
        bool Insertar(Modelos.Entidades.VentaCruzada modelo);
        bool Actualizar(Modelos.Entidades.VentaCruzada modelo);
        bool Eliminar(int id);
        Modelos.Entidades.VentaCruzada? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.VentaCruzada> ObtenerTodos();
        IQueryable<Modelos.Entidades.VentaCruzada> Query();
    }
}
