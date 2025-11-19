using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICobranzaPagoRepository
    {
        IQueryable<CobranzaPago> Query();
        CobranzaPago? ObtenerPorId(int id);
        bool Insertar(CobranzaPago modelo);
        bool Actualizar(CobranzaPago modelo);
    }

}
