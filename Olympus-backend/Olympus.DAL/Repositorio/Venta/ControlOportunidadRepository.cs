using CapaDatos.DataContext;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Venta
{
    public class ControlOportunidadRepository : IControlOportunidadRepository
    {
        private readonly OlympusContext _context;

        public ControlOportunidadRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(ControlOportunidad modelo)
        {
            _context.ControlOportunidad.Add(modelo);
            return true;
        }

        public bool Actualizar(ControlOportunidad modelo)
        {
            _context.ControlOportunidad.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.ControlOportunidad.FirstOrDefault(c => c.Id == id);
            if (ent == null) return false;
            _context.ControlOportunidad.Remove(ent);
            return true;
        }

        public ControlOportunidad? ObtenerPorId(int id)
        {
            return _context.ControlOportunidad.FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<ControlOportunidad> ObtenerTodos()
        {
            return _context.ControlOportunidad.AsQueryable();
        }
    }
}
