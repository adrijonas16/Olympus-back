using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IOcurrenciaRepository
    {
        bool Insertar(Modelos.Entidades.Ocurrencia modelo);
        bool Actualizar(Modelos.Entidades.Ocurrencia modelo);
        bool Eliminar(int id);
        Modelos.Entidades.Ocurrencia? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.Ocurrencia> ObtenerTodos();
        IQueryable<Modelos.Entidades.Ocurrencia> Query();
    }
}
