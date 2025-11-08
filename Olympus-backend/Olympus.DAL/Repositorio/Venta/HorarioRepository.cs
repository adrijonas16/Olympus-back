using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly OlympusContext _context;

        public HorarioRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Horario modelo)
        {
            _context.Horario.Add(modelo);
            return true;
        }

        public bool Actualizar(Horario modelo)
        {
            _context.Horario.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Horario.FirstOrDefault(h => h.Id == id);
            if (ent == null) return false;
            _context.Horario.Remove(ent);
            return true;
        }

        public Horario? ObtenerPorId(int id)
        {
            return _context.Horario
                .Include(h => h.Producto)
                .FirstOrDefault(h => h.Id == id);
        }

        public IQueryable<Horario> Query()
        {
            return _context.Horario.AsNoTracking().AsQueryable();
        }
        public IQueryable<Horario> ObtenerTodos()
        {
            return _context.Horario.AsNoTracking().AsQueryable();
        }
    }
}
