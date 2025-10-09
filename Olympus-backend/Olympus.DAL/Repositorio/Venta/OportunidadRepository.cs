using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class OportunidadRepository : IOportunidadRepository
    {
        private readonly OlympusContext _context;

        public OportunidadRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Oportunidad modelo)
        {
            _context.Oportunidad.Add(modelo);
            return true;
        }

        public bool Actualizar(Oportunidad modelo)
        {
            _context.Oportunidad.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Oportunidad.FirstOrDefault(o => o.Id == id);
            if (ent == null) return false;
            _context.Oportunidad.Remove(ent);
            return true;
        }

        public Oportunidad? ObtenerPorId(int id)
        {
            return _context.Oportunidad.FirstOrDefault(o => o.Id == id);
        }

        public IQueryable<Oportunidad> ObtenerTodos()
        {
            return _context.Oportunidad.AsQueryable();
        }
        // Trae la oportunidad incluyendo la Persona (EAGER LOAD).
        public Oportunidad? ObtenerPorIdConPersona(int id)
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Include(o => o.Persona)
                .FirstOrDefault(o => o.Id == id);
        }

        // Devuelve la Persona asociada a una oportunidad
        public Persona? ObtenerPersonaPorOportunidad(int idOportunidad)
        {
            // EF traduce a un JOIN SELECT y solo trae columnas de Persona.
            return _context.Oportunidad
                .AsNoTracking()
                .Where(o => o.Id == idOportunidad)
                .Select(o => o.Persona)
                .FirstOrDefault();
        }

        // Devuelve la lista de personas relacionadas a un conjunto de oportunidades
        public List<Persona> ObtenerPersonasPorOportunidades(IEnumerable<int> idsOportunidad)
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Where(o => idsOportunidad.Contains(o.Id))
                .Select(o => o.Persona)
                .Where(p => p != null)
                .Distinct()
                .ToList()!;
        }

        // Devuelve todas las oportunidades con su persona
        public IQueryable<Oportunidad> ObtenerTodosConPersona()
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Include(o => o.Persona);
        }
    }
}
