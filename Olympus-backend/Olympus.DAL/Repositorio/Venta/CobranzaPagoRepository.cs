using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Venta
{
    public class CobranzaPagoRepository : ICobranzaPagoRepository
    {
        private readonly OlympusContext _context;
        public CobranzaPagoRepository(OlympusContext context) => _context = context;

        public IQueryable<CobranzaPago> Query()
        {
            return _context.CobranzaPago.AsNoTracking().AsQueryable();
        }

        public CobranzaPago? ObtenerPorId(int id)
        {
            return _context.CobranzaPago
                .Include(p => p.Aplicaciones)
                .FirstOrDefault(p => p.Id == id);
        }

        public bool Insertar(CobranzaPago modelo)
        {
            _context.CobranzaPago.Add(modelo);
            return true;
        }

        public bool Actualizar(CobranzaPago modelo)
        {
            _context.CobranzaPago.Update(modelo);
            return true;
        }
    }
}
