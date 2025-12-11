using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class MetodoPagoProductoRepository : IMetodoPagoProductoRepository
    {
        private readonly OlympusContext _context;

        public MetodoPagoProductoRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Insertar(MetodoPagoProducto modelo)
        {
            _context.MetodoPagoProducto.Add(modelo);
            return true;
        }
        public bool Actualizar(MetodoPagoProducto modelo)
        {
            _context.MetodoPagoProducto.Update(modelo);
            return true;
        }
        public bool Eliminar(int id)
        {
            var ent = _context.MetodoPagoProducto.FirstOrDefault(x => x.Id == id);
            if (ent == null) return false;
            _context.MetodoPagoProducto.Remove(ent);
            return true;
        }
        public MetodoPagoProducto? ObtenerPorId(int id)
        {
            return _context.MetodoPagoProducto
                .Include(mp => mp.Producto)
                .Include(mp => mp.MetodoPago)
                .FirstOrDefault(mp => mp.Id == id);
        }
        public IQueryable<MetodoPagoProducto> Query()
        {
            return _context.MetodoPagoProducto.AsNoTracking().AsQueryable();
        }
        public IQueryable<MetodoPagoProducto> ObtenerTodos()
        {
            return _context.MetodoPagoProducto.AsNoTracking().AsQueryable();
        }
    }
}
