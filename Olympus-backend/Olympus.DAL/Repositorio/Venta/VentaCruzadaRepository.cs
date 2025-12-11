using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class VentaCruzadaRepository : IVentaCruzadaRepository
    {
        private readonly OlympusContext _context;

        public VentaCruzadaRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(VentaCruzada modelo)
        {
            _context.VentaCruzada.Add(modelo);
            return true;
        }

        public bool Actualizar(VentaCruzada modelo)
        {
            _context.VentaCruzada.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.VentaCruzada.FirstOrDefault(v => v.Id == id);
            if (ent == null) return false;
            _context.VentaCruzada.Remove(ent);
            return true;
        }

        public VentaCruzada? ObtenerPorId(int id)
        {
            return _context.VentaCruzada
                .AsNoTracking()
                .Include(v => v.ProductoOrigen)
                .Include(v => v.ProductoDestino)
                .Include(v => v.HistorialEstado)
                .FirstOrDefault(v => v.Id == id);
        }

        public IQueryable<VentaCruzada> Query()
        {
            return _context.VentaCruzada.AsNoTracking().AsQueryable();
        }
        public IQueryable<VentaCruzada> ObtenerTodos()
        {
            return _context.VentaCruzada.AsNoTracking().AsQueryable();
        }
    }
}
