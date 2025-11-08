using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class HistorialEstadoTipoRepository : IHistorialEstadoTipoRepository
    {
        private readonly OlympusContext _context;

        public HistorialEstadoTipoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(HistorialEstadoTipo modelo)
        {
            _context.HistorialEstadoTipo.Add(modelo);
            return true;
        }

        public bool Actualizar(HistorialEstadoTipo modelo)
        {
            _context.HistorialEstadoTipo.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.HistorialEstadoTipo.FirstOrDefault(h => h.Id == id);
            if (ent == null) return false;
            _context.HistorialEstadoTipo.Remove(ent);
            return true;
        }

        public HistorialEstadoTipo? ObtenerPorId(int id)
        {
            return _context.HistorialEstadoTipo
                .Include(h => h.Tipo)
                .Include(h => h.HistorialEstado)
                .FirstOrDefault(h => h.Id == id);
        }

        public IQueryable<HistorialEstadoTipo> Query()
        {
            return _context.HistorialEstadoTipo.AsNoTracking().AsQueryable();
        }
        public IQueryable<HistorialEstadoTipo> ObtenerTodos()
        {
            return _context.HistorialEstadoTipo.AsNoTracking().AsQueryable();
        }
    }
}
