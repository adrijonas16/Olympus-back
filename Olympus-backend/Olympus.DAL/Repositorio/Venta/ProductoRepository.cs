using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly OlympusContext _context;

        public ProductoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Producto modelo)
        {
            _context.Producto.Add(modelo);
            return true;
        }

        public bool Actualizar(Producto modelo)
        {
            _context.Producto.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Producto.FirstOrDefault(p => p.Id == id);
            if (ent == null) return false;
            _context.Producto.Remove(ent);
            return true;
        }

        public Producto? ObtenerPorId(int id)
        {
            return _context.Producto
                .AsNoTracking()
                .Include(p => p.Lanzamiento)
                .Include(p => p.Horarios)
                .Include(p => p.Inversiones)
                    .ThenInclude(inv => inv.Descuentos)              // incluye descuentos de cada inversion
                .Include(p => p.MetodoPagoProductos)
                    .ThenInclude(mpp => mpp.MetodoPago)              // incluye MetodoPago desde MetodoPagoProducto
                .Include(p => p.ProductoCertificados)
                    .ThenInclude(pc => pc.Certificado)              // incluye Certificado desde ProductoCertificado
                .Include(p => p.Beneficios)
                .Include(p => p.Cobranzas)
                    .ThenInclude(c => c.Inversion)                  // incluye Inversion desde Cobranza
                .Include(p => p.Cobranzas)
                    .ThenInclude(c => c.HistorialEstado)           // incluye HistorialEstado desde Cobranza
                .Include(p => p.Convertidos)
                    .ThenInclude(conv => conv.Inversion)           // incluye Inversion desde Convertido
                .Include(p => p.VentaCruzadaOrigen)
                    .ThenInclude(vc => vc.ProductoDestino)         // incluye ProductoDestino en ventas cruzadas origen
                .Include(p => p.VentaCruzadaDestino)
                    .ThenInclude(vc => vc.ProductoOrigen)          // incluye ProductoOrigen en ventas cruzadas destino
                .Include(p => p.Corporativos)
                    .ThenInclude(cor => cor.HistorialEstado)       // incluye HistorialEstado en Corporativo
                .FirstOrDefault(p => p.Id == id);
        }
        public IQueryable<Producto> Query()
        {
            return _context.Producto.AsNoTracking().AsQueryable();
        }

        public IQueryable<Producto> ObtenerTodos()
        {
            return _context.Producto.AsNoTracking().AsQueryable();
        }
    }
}
