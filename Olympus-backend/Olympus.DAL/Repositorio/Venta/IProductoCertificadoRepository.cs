using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public interface IProductoCertificadoRepository
    {
        bool Insertar(Modelos.Entidades.ProductoCertificado modelo);
        bool Actualizar(Modelos.Entidades.ProductoCertificado modelo);
        bool Eliminar(int id);
        Modelos.Entidades.ProductoCertificado? ObtenerPorId(int id);
        IQueryable<Modelos.Entidades.ProductoCertificado> ObtenerTodos();
        IQueryable<Modelos.Entidades.ProductoCertificado> Query();
    }
}
