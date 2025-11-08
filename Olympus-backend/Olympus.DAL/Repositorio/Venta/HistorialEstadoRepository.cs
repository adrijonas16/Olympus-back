using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class HistorialEstadoRepository : IHistorialEstadoRepository
    {
        private readonly OlympusContext _context;

        public HistorialEstadoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(HistorialEstado modelo)
        {
            _context.HistorialEstado.Add(modelo);
            return true;
        }

        public bool Actualizar(HistorialEstado modelo)
        {
            _context.HistorialEstado.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.HistorialEstado.FirstOrDefault(h => h.Id == id);
            if (ent == null) return false;
            _context.HistorialEstado.Remove(ent);
            return true;
        }

        public HistorialEstado? ObtenerPorId(int id)
        {
            return _context.HistorialEstado.FirstOrDefault(h => h.Id == id);
        }

        public IQueryable<HistorialEstado> ObtenerTodos()
        {
            return _context.HistorialEstado.AsNoTracking().AsQueryable();
        }
        public IQueryable<HistorialEstado> Query()
        {
            return _context.HistorialEstado.AsNoTracking().AsQueryable();
        }
    }
}
