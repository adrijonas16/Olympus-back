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
    public class CobranzaPlanRepository : ICobranzaPlanRepository
    {
        private readonly OlympusContext _context;
        public CobranzaPlanRepository(OlympusContext context) => _context = context;
        public IQueryable<CobranzaPlan> Query() => _context.CobranzaPlan.AsNoTracking();
        public CobranzaPlan? ObtenerPorId(int id) => _context.CobranzaPlan.Include(p => p.Cuotas).FirstOrDefault(p => p.Id == id);
        public bool Insertar(CobranzaPlan modelo) { _context.CobranzaPlan.Add(modelo); return true; }
        public bool Actualizar(CobranzaPlan modelo) { _context.CobranzaPlan.Update(modelo); return true; }
    }

}
