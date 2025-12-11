using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public interface ICertificadoRepository
    {
        bool Insertar(Certificado modelo);
        bool Actualizar(Certificado modelo);
        bool Eliminar(int id);
        Certificado? ObtenerPorId(int id);
        IQueryable<Certificado> Query();
        IQueryable<Certificado> ObtenerTodos();
        
    }
}
