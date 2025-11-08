using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class CobranzaRepository : ICobranzaRepository
    {
        private readonly OlympusContext _context;

        public CobranzaRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Cobranza modelo)
        {
            _context.Cobranza.Add(modelo);
            return true;
        }

        public bool Actualizar(Cobranza modelo)
        {
            _context.Cobranza.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Cobranza.FirstOrDefault(c => c.Id == id);
            if (ent == null) return false;
            _context.Cobranza.Remove(ent);
            return true;
        }

        public Cobranza? ObtenerPorId(int id)
        {
            return _context.Cobranza
                .Include(c => c.HistorialEstado)
                .Include(c => c.Inversion)
                .FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<Cobranza> Query()
        {
            return _context.Cobranza.AsNoTracking().AsQueryable();
        }

        public IQueryable<Cobranza> ObtenerTodos()
        {
            return _context.Cobranza.AsNoTracking().AsQueryable();
        }
    }
}
