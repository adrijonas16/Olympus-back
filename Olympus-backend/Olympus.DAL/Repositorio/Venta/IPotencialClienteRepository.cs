using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IPotencialClienteRepository
    {
        IQueryable<PotencialCliente> ObtenerTodos();
        PotencialCliente? ObtenerPorId(int id);
        bool Insertar(PotencialCliente entidad);
        bool Actualizar(PotencialCliente entidad);
        bool Eliminar(int id);
        IQueryable<PotencialCliente> Query();
    }
}
