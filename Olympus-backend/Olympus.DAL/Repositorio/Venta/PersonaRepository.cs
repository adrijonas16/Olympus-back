using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class PersonaRepository : IPersonaRepository
    {
        private readonly OlympusContext _context;
        public PersonaRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Persona modelo)
        {
            _context.Persona.Add(modelo);
            return true;
        }

        public bool Actualizar(Persona modelo)
        {
            _context.Persona.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Persona.FirstOrDefault(p => p.Id == id);
            if (ent == null) return false;
            _context.Persona.Remove(ent);
            return true;
        }

        public Persona? ObtenerPorId(int id)
        {
            return _context.Persona
                .Include(p => p.Pais)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.Id == id);
        }

        public IQueryable<Persona> ObtenerTodos()
        {
            return _context.Persona
                .Include(p => p.Pais)
                .Include(p => p.Usuario);
        }

        public Persona? ObtenerPorIdUsuario(int idUsuario)
        {
            return _context.Persona
                .Include(p => p.Pais)
                .Include(p => p.Usuario)
                .FirstOrDefault(p => p.IdUsuario == idUsuario);
        }


    }
}
