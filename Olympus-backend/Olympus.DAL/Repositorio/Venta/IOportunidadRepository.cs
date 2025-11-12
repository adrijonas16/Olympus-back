using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public interface IOportunidadRepository
    {
        bool Insertar(Oportunidad modelo);
        bool Actualizar(Oportunidad modelo);
        bool Eliminar(int id);
        Oportunidad? ObtenerPorId(int id);
        PotencialCliente? ObtenerPersonaPorOportunidad(int idOportunidad);
        List<Persona> ObtenerPersonasPorOportunidades(IEnumerable<int> idsOportunidad);
        IQueryable<Oportunidad> ObtenerTodosConPersona();
        IQueryable<Oportunidad> ObtenerTodas();
        IQueryable<Oportunidad> Query();
        IQueryable<Oportunidad> QueryAsNoTracking();
    }
}
