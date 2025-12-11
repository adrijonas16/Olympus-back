using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IPaisRepository
    {
        IQueryable<Pais> ObtenerTodos();
        Pais? ObtenerPorId(int id);
        bool Insertar(Pais entidad);
        bool Actualizar(Pais entidad);
        bool Eliminar(int id);
    }
}
