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
    public class PotencialClienteRepository : IPotencialClienteRepository
    {
        private readonly OlympusContext _context;

        public PotencialClienteRepository(OlympusContext context)
        {
            _context = context;
        }

        public IQueryable<PotencialCliente> ObtenerTodos()
        {
            return _context.PotencialCliente.AsNoTracking().AsQueryable();
        }

        public PotencialCliente? ObtenerPorId(int id)
        {
            return _context.PotencialCliente
                           .AsNoTracking()
                           .FirstOrDefault(p => p.Id == id);
        }

        public bool Insertar(PotencialCliente entidad)
        {
            _context.PotencialCliente.Add(entidad);
            return true;
        }

        public bool Actualizar(PotencialCliente entidad)
        {
            _context.PotencialCliente.Update(entidad);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.PotencialCliente.FirstOrDefault(p => p.Id == id);
            if (ent == null) return false;
            _context.PotencialCliente.Remove(ent);
            return true;
        }
        public IQueryable<PotencialCliente> Query()
        {
            return _context.PotencialCliente.AsNoTracking().AsQueryable();
        }
    }
}
