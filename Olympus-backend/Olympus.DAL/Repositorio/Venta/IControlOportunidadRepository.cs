using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IControlOportunidadRepository
    {
        bool Insertar(ControlOportunidad modelo);
        bool Actualizar(ControlOportunidad modelo);
        bool Eliminar(int id);
        ControlOportunidad? ObtenerPorId(int id);
        IQueryable<ControlOportunidad> ObtenerTodos();
    }
}
