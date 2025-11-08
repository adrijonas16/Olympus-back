using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly OlympusContext _context;

        public MetodoPagoRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Insertar(MetodoPago modelo)
        {
            _context.MetodoPago.Add(modelo);
            return true;
        }
        public bool Actualizar(MetodoPago modelo)
        {
            _context.MetodoPago.Update(modelo);
            return true;
        }
        public bool Eliminar(int id)
        {
            var ent = _context.MetodoPago.FirstOrDefault(m => m.Id == id);
            if (ent == null) return false;
            _context.MetodoPago.Remove(ent);
            return true;
        }
        public MetodoPago? ObtenerPorId(int id)
        {
            return _context.MetodoPago
                .Include(m => m.MetodoPagoProductos)
                .FirstOrDefault(m => m.Id == id);
        }
        public IQueryable<MetodoPago> ObtenerTodos()
        {
            return _context.MetodoPago.AsNoTracking().AsQueryable();
        }

        public IQueryable<MetodoPago> Query()
        {
            return _context.MetodoPago.AsNoTracking().AsQueryable();
        }
    }
}
