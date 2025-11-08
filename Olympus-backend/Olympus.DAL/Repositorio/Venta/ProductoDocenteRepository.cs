using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class ProductoDocenteRepository : IProductoDocenteRepository
    {
        private readonly OlympusContext _context;

        public ProductoDocenteRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(ProductoDocente modelo)
        {
            _context.ProductoDocente.Add(modelo);
            return true;
        }

        public bool Actualizar(ProductoDocente modelo)
        {
            _context.ProductoDocente.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.ProductoDocente.FirstOrDefault(pd => pd.Id == id);
            if (ent == null) return false;
            _context.ProductoDocente.Remove(ent);
            return true;
        }

        public ProductoDocente? ObtenerPorId(int id)
        {
            return _context.ProductoDocente
                .Include(pd => pd.Producto)
                .Include(pd => pd.Docente)
                .FirstOrDefault(pd => pd.Id == id);
        }

        public IQueryable<ProductoDocente> Query()
        {
            return _context.ProductoDocente.AsNoTracking().AsQueryable();
        }

        public IQueryable<ProductoDocente> ObtenerTodos()
        {
            return _context.ProductoDocente.AsQueryable();
        }
    }
}
