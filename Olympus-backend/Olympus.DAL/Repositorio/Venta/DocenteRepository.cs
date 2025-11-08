using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class DocenteRepository : IDocenteRepository
    {
        private readonly OlympusContext _context;

        public DocenteRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Docente modelo)
        {
            _context.Docente.Add(modelo);
            return true;
        }

        public bool Actualizar(Docente modelo)
        {
            _context.Docente.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Docente.FirstOrDefault(d => d.Id == id);
            if (ent == null) return false;
            _context.Docente.Remove(ent);
            return true;
        }

        public Docente? ObtenerPorId(int id)
        {
            return _context.Docente
                .Include(d => d.Persona)
                .FirstOrDefault(d => d.Id == id);
        }

        public IQueryable<Docente> Query()
        {
            return _context.Docente.AsNoTracking().AsQueryable();
        }
        public IQueryable<Docente> ObtenerTodos()
        {
            return _context.Docente.AsNoTracking().AsQueryable();
        }
    }
}
