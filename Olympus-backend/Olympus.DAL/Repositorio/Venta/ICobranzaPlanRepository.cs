using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICobranzaPlanRepository
    {
        IQueryable<CobranzaPlan> Query();
        CobranzaPlan? ObtenerPorId(int id);
        bool Insertar(CobranzaPlan modelo);
        bool Actualizar(CobranzaPlan modelo);
    }
}
