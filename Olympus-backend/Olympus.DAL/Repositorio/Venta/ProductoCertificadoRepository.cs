using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class ProductoCertificadoRepository : IProductoCertificadoRepository
    {
        private readonly OlympusContext _context;

        public ProductoCertificadoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(ProductoCertificado modelo)
        {
            _context.ProductoCertificado.Add(modelo);
            return true;
        }

        public bool Actualizar(ProductoCertificado modelo)
        {
            _context.ProductoCertificado.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.ProductoCertificado.FirstOrDefault(pc => pc.Id == id);
            if (ent == null) return false;
            _context.ProductoCertificado.Remove(ent);
            return true;
        }

        public ProductoCertificado? ObtenerPorId(int id)
        {
            return _context.ProductoCertificado
                .Include(pc => pc.Producto)
                .Include(pc => pc.Certificado)
                .FirstOrDefault(pc => pc.Id == id);
        }

        public IQueryable<ProductoCertificado> Query()
        {
            return _context.ProductoCertificado.AsQueryable();
        }

        // Alternativa con nombre clásico (opcional)
        public IQueryable<ProductoCertificado> ObtenerTodos()
        {
            return _context.ProductoCertificado.AsQueryable();
        }
    }
}
