using CapaDatos.DataContext;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class MotivoRepository : IMotivoRepository
    {
        private readonly OlympusContext _context;

        public MotivoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Motivo modelo)
        {
            _context.Motivo.Add(modelo);
            return true;
        }

        public bool Actualizar(Motivo modelo)
        {
            _context.Motivo.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Motivo.FirstOrDefault(m => m.Id == id);
            if (ent == null) return false;
            _context.Motivo.Remove(ent);
            return true;
        }

        public Motivo? ObtenerPorId(int id)
        {
            return _context.Motivo.FirstOrDefault(m => m.Id == id);
        }

        public IQueryable<Motivo> ObtenerTodos()
        {
            return _context.Motivo.AsQueryable();
        }
    }
}
