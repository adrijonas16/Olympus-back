using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class AsesorRepository : IAsesorRepository
    {
        private readonly OlympusContext _context;

        public AsesorRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Asesor modelo)
        {
            _context.Asesor.Add(modelo);
            return true;
        }

        public bool Actualizar(Asesor modelo)
        {
            _context.Asesor.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Asesor.FirstOrDefault(a => a.Id == id);
            if (ent == null) return false;
            _context.Asesor.Remove(ent);
            return true;
        }

        public Asesor? ObtenerPorId(int id)
        {
            return _context.Asesor
               .Include(p => p.Pais)
               .FirstOrDefault(p => p.Id == id);
        }

        public Asesor? ObtenerPorIdPersona(int idPersona)
        {
            return _context.Asesor.FirstOrDefault(h => h.IdPersona == idPersona);
        }


        public IQueryable<Asesor> ObtenerTodos()
        {
            return _context.Asesor.AsQueryable();
        }

        public IQueryable<Asesor> Query()
        {
            return _context.Asesor.AsNoTracking().AsQueryable();
        }
    }
}
