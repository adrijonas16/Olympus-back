using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IMotivoRepository
    {
        bool Insertar(Motivo modelo);
        bool Actualizar(Motivo modelo);
        bool Eliminar(int id);
        Motivo? ObtenerPorId(int id);
        IQueryable<Motivo> ObtenerTodos();
    }
}
