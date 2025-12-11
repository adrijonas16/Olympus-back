using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IAsesorRepository
    {
        bool Insertar(Asesor modelo);
        bool Actualizar(Asesor modelo);
        bool Eliminar(int id);
        Asesor? ObtenerPorId(int id);
        Asesor? ObtenerPorIdPersona(int idPersona);
        IQueryable<Asesor> ObtenerTodos();
        IQueryable<Asesor> Query();
    }
}
