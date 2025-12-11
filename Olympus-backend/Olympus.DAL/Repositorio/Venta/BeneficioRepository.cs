using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class BeneficioRepository : IBeneficioRepository
    {
        private readonly OlympusContext _context;

        public BeneficioRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Beneficio modelo)
        {
            _context.Beneficio.Add(modelo);
            return true;
        }

        public bool Actualizar(Beneficio modelo)
        {
            _context.Beneficio.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Beneficio.FirstOrDefault(b => b.Id == id);
            if (ent == null) return false;
            _context.Beneficio.Remove(ent);
            return true;
        }

        public Beneficio? ObtenerPorId(int id)
        {
            return _context.Beneficio
                .Include(b => b.Producto)
                .FirstOrDefault(b => b.Id == id);
        }

        public IQueryable<Beneficio> Query()
        {
            return _context.Beneficio.AsNoTracking().AsQueryable();
        }

        public IQueryable<Beneficio> ObtenerTodos()
        {
            return _context.Beneficio.AsNoTracking().AsQueryable();
        }
    }
}
