using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class ConvertidoRepository : IConvertidoRepository
    {
        private readonly OlympusContext _context;

        public ConvertidoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Convertido modelo)
        {
            _context.Convertido.Add(modelo);
            return true;
        }

        public bool Actualizar(Convertido modelo)
        {
            _context.Convertido.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Convertido.FirstOrDefault(c => c.Id == id);
            if (ent == null) return false;
            _context.Convertido.Remove(ent);
            return true;
        }

        public Convertido? ObtenerPorId(int id)
        {
            return _context.Convertido
                .Include(c => c.HistorialEstado)
                .Include(c => c.Inversion)
                .Include(c => c.Producto)
                .FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<Convertido> Query()
        {
            return _context.Convertido.AsQueryable();
        }
        public IQueryable<Convertido> ObtenerTodos()
        {
            return _context.Convertido.AsNoTracking().AsQueryable();
        }
    }
}
