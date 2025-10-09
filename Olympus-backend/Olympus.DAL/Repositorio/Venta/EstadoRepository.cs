using CapaDatos.DataContext;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly OlympusContext _context;

        public EstadoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Estado modelo)
        {
            _context.Estado.Add(modelo);
            return true;
        }

        public bool Actualizar(Estado modelo)
        {
            _context.Estado.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Estado.FirstOrDefault(e => e.Id == id);
            if (ent == null) return false;
            _context.Estado.Remove(ent);
            return true;
        }

        public Estado? ObtenerPorId(int id)
        {
            return _context.Estado.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<Estado> ObtenerTodos()
        {
            return _context.Estado.AsQueryable();
        }
    }
}
