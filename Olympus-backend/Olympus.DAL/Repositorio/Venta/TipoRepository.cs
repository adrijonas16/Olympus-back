using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class TipoRepository : ITipoRepository
    {
        private readonly OlympusContext _context;

        public TipoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Tipo modelo)
        {
            _context.Tipo.Add(modelo);
            return true;
        }

        public bool Actualizar(Tipo modelo)
        {
            _context.Tipo.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Tipo.FirstOrDefault(t => t.Id == id);
            if (ent == null) return false;
            _context.Tipo.Remove(ent);
            return true;
        }

        public Tipo? ObtenerPorId(int id)
        {
            return _context.Tipo
                .Include(t => t.Estados)               // relacion  a Estado
                .Include(t => t.HistorialEstadoTipos) // relacion a HistorialEstadoTipo
                .FirstOrDefault(t => t.Id == id);
        }

        public IQueryable<Tipo> Query()
        {
            return _context.Tipo.AsNoTracking().AsQueryable();
        }

        public IQueryable<Tipo> ObtenerTodos()
        {
            return _context.Tipo.AsNoTracking().AsQueryable();
        }
    }
}
