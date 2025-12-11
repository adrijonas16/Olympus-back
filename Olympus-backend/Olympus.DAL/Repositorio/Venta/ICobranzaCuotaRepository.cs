using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICobranzaCuotaRepository
    {
        IQueryable<CobranzaCuota> Query();
        CobranzaCuota? ObtenerPorId(int id);
        bool Insertar(CobranzaCuota modelo);
        bool Actualizar(CobranzaCuota modelo);
    }
}
