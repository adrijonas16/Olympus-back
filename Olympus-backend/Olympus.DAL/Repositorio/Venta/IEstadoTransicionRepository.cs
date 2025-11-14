using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IEstadoTransicionRepository
    {
        bool Insertar(EstadoTransicion modelo);
        bool Actualizar(EstadoTransicion modelo);
        bool Eliminar(int id);
        EstadoTransicion? ObtenerPorId(int id);
        IQueryable<EstadoTransicion> Query();
        IQueryable<EstadoTransicion> ObtenerTodos();
    }
}
