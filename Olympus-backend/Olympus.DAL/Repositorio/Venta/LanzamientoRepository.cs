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
    public class LanzamientoRepository : ILanzamientoRepository
    {
        private readonly OlympusContext _context;

        public LanzamientoRepository(OlympusContext context)
        {
            _context = context;
        }

        public IQueryable<Lanzamiento> ObtenerTodos()
        {
            return _context.Lanzamiento.AsNoTracking().AsQueryable();
        }

        public Lanzamiento? ObtenerPorId(int id)
        {
            return _context.Lanzamiento
                           .AsNoTracking()
                           .FirstOrDefault(l => l.Id == id);
        }

        public bool Insertar(Lanzamiento entidad)
        {
            _context.Lanzamiento.Add(entidad);
            return true;
        }

        public bool Actualizar(Lanzamiento entidad)
        {
            _context.Lanzamiento.Update(entidad);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Lanzamiento.FirstOrDefault(l => l.Id == id);
            if (ent == null) return false;
            _context.Lanzamiento.Remove(ent);
            return true;
        }
    }
}
