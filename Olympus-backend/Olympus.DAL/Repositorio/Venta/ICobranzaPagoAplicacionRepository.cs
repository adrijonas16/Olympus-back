using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICobranzaPagoAplicacionRepository
    {
        IQueryable<CobranzaPagoAplicacion> Query();
        CobranzaPagoAplicacion? ObtenerPorId(int id);
        bool Insertar(CobranzaPagoAplicacion modelo);
        bool Actualizar(CobranzaPagoAplicacion modelo);
    }
}
