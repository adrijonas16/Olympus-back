using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class InversionRepository : IInversionRepository
    {
        private readonly OlympusContext _context;

        public InversionRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Inversion modelo)
        {
            _context.Inversion.Add(modelo);
            return true;
        }

        public bool Actualizar(Inversion modelo)
        {
            _context.Inversion.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Inversion.FirstOrDefault(x => x.Id == id);
            if (ent == null) return false;
            _context.Inversion.Remove(ent);
            return true;
        }

        public Inversion? ObtenerPorId(int id)
        {
            return _context.Inversion
                .Include(i => i.Producto)
                .Include(i => i.Oportunidad)
                .Include(i => i.Descuentos)
                .Include(i => i.Cobranzas)
                .Include(i => i.Convertidos)
                .FirstOrDefault(i => i.Id == id);
        }

        public IQueryable<Inversion> Query()
        {
            return _context.Inversion.AsNoTracking().AsQueryable();
        }

        public IQueryable<Inversion> ObtenerTodos()
        {
            return _context.Inversion.AsNoTracking().AsQueryable();
        }
    }
}
