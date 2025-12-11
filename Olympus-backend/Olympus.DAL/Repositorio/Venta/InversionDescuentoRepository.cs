using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class InversionDescuentoRepository : IInversionDescuentoRepository
    {
        private readonly OlympusContext _context;

        public InversionDescuentoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(InversionDescuento modelo)
        {
            _context.InversionDescuento.Add(modelo);
            return true;
        }

        public bool Actualizar(InversionDescuento modelo)
        {
            _context.InversionDescuento.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.InversionDescuento.FirstOrDefault(d => d.Id == id);
            if (ent == null) return false;
            _context.InversionDescuento.Remove(ent);
            return true;
        }

        public InversionDescuento? ObtenerPorId(int id)
        {
            return _context.InversionDescuento
                .Include(d => d.Tipo)
                .Include(d => d.Inversion)
                .FirstOrDefault(d => d.Id == id);
        }

        public IQueryable<InversionDescuento> Query()
        {
            return _context.InversionDescuento.AsNoTracking().AsQueryable();
        }
        public IQueryable<InversionDescuento> ObtenerTodos()
        {
            return _context.InversionDescuento.AsNoTracking().AsQueryable();
        }
    }
}
