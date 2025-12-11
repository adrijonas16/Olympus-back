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
    public class PaisRepository : IPaisRepository
    {
        private readonly OlympusContext _context;

        public PaisRepository(OlympusContext context)
        {
            _context = context;
        }

        public IQueryable<Pais> ObtenerTodos() 
        {
            return _context.Pais.AsNoTracking().AsQueryable();
        }

        public Pais? ObtenerPorId(int id)
        {
            return _context.Pais
                           .AsNoTracking()
                           .FirstOrDefault(p => p.Id == id);
        }

        public bool Insertar(Pais entidad)
        {
            _context.Pais.Add(entidad);
            return true;
        }

        public bool Actualizar(Pais entidad)
        {
            _context.Pais.Update(entidad);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Pais.FirstOrDefault(p => p.Id == id);
            if (ent == null) return false;
            _context.Pais.Remove(ent);
            return true;
        }
    }
}
