using CapaDatos.DataContext;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class HistorialInteraccionRepository : IHistorialInteraccionRepository
    {
        private readonly OlympusContext _context;

        public HistorialInteraccionRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(HistorialInteraccion modelo)
        {
            _context.HistorialInteraccion.Add(modelo);
            return true;
        }

        public bool Actualizar(HistorialInteraccion modelo)
        {
            _context.HistorialInteraccion.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.HistorialInteraccion.FirstOrDefault(h => h.Id == id);
            if (ent == null) return false;
            _context.HistorialInteraccion.Remove(ent);
            return true;
        }

        public HistorialInteraccion? ObtenerPorId(int id)
        {
            return _context.HistorialInteraccion.FirstOrDefault(h => h.Id == id);
        }

        public IQueryable<HistorialInteraccion> ObtenerTodos()
        {
            return _context.HistorialInteraccion.AsQueryable();
        }

        public IQueryable<HistorialInteraccion> Query()
        {
            return _context.HistorialInteraccion.AsQueryable();
        }
    }
}
