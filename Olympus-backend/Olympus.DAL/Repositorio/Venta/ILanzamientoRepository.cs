using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ILanzamientoRepository
    {
        IQueryable<Lanzamiento> ObtenerTodos();
        Lanzamiento? ObtenerPorId(int id);
        bool Insertar(Lanzamiento entidad);
        bool Actualizar(Lanzamiento entidad);
        bool Eliminar(int id);
    }
}
