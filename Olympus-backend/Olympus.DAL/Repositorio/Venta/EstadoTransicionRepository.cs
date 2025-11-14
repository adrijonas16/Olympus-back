using CapaDatos.DataContext;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class EstadoTransicionRepository : IEstadoTransicionRepository
    {
        private readonly OlympusContext _context;

        public EstadoTransicionRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(EstadoTransicion modelo)
        {
            _context.EstadoTransicion.Add(modelo);
            return true;
        }

        public bool Actualizar(EstadoTransicion modelo)
        {
            _context.EstadoTransicion.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.EstadoTransicion.FirstOrDefault(e => e.Id == id);
            if (ent == null) return false;
            _context.EstadoTransicion.Remove(ent);
            return true;
        }

        public EstadoTransicion? ObtenerPorId(int id)
        {
            return _context.EstadoTransicion.FirstOrDefault(e => e.Id == id);
        }

        public IQueryable<EstadoTransicion> ObtenerTodos()
        {
            return _context.EstadoTransicion.AsQueryable();
        }

        public IQueryable<EstadoTransicion> Query()
        {
            return _context.EstadoTransicion.AsQueryable();
        }
    }
}
