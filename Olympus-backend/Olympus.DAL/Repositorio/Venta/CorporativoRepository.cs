using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class CorporativoRepository : ICorporativoRepository
    {
        private readonly OlympusContext _context;

        public CorporativoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Corporativo modelo)
        {
            _context.Corporativo.Add(modelo);
            return true;
        }

        public bool Actualizar(Corporativo modelo)
        {
            _context.Corporativo.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Corporativo.FirstOrDefault(c => c.Id == id);
            if (ent == null) return false;
            _context.Corporativo.Remove(ent);
            return true;
        }

        public Corporativo? ObtenerPorId(int id)
        {
            return _context.Corporativo
                .Include(c => c.Producto)
                .Include(c => c.HistorialEstado)
                .FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<Corporativo> Query()
        {
            return _context.Corporativo.AsQueryable();
        }

        public IQueryable<Corporativo> ObtenerTodos()
        {
            return _context.Corporativo.AsNoTracking().AsQueryable();
        }
    }
}
