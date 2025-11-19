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
    public class CobranzaCuotaRepository : ICobranzaCuotaRepository
    {
        private readonly OlympusContext _context;
        public CobranzaCuotaRepository(OlympusContext context) => _context = context;

        public IQueryable<CobranzaCuota> Query()
        {
            return _context.CobranzaCuota.AsNoTracking().AsQueryable();
        }

        public CobranzaCuota? ObtenerPorId(int id)
        {
            return _context.CobranzaCuota
                .Include(c => c.Aplicaciones)
                .FirstOrDefault(c => c.Id == id);
        }

        public bool Insertar(CobranzaCuota modelo)
        {
            _context.CobranzaCuota.Add(modelo);
            return true;
        }

        public bool Actualizar(CobranzaCuota modelo)
        {
            _context.CobranzaCuota.Update(modelo);
            return true;
        }
    }
}
