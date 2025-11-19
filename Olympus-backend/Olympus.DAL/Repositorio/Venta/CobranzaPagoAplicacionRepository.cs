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
    public class CobranzaPagoAplicacionRepository : ICobranzaPagoAplicacionRepository
    {
        private readonly OlympusContext _context;
        public CobranzaPagoAplicacionRepository(OlympusContext context) => _context = context;

        public IQueryable<CobranzaPagoAplicacion> Query()
        {
            return _context.CobranzaPagoAplicacion.AsNoTracking().AsQueryable();
        }

        public CobranzaPagoAplicacion? ObtenerPorId(int id)
        {
            return _context.CobranzaPagoAplicacion
                .FirstOrDefault(a => a.Id == id);
        }

        public bool Insertar(CobranzaPagoAplicacion modelo)
        {
            _context.CobranzaPagoAplicacion.Add(modelo);
            return true;
        }

        public bool Actualizar(CobranzaPagoAplicacion modelo)
        {
            _context.CobranzaPagoAplicacion.Update(modelo);
            return true;
        }
    }
}
