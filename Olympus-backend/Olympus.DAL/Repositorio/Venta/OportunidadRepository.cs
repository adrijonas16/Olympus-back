using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class OportunidadRepository : IOportunidadRepository
    {
        private readonly OlympusContext _context;

        public OportunidadRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Oportunidad modelo)
        {
            _context.Oportunidad.Add(modelo);
            return true;
        }

        public bool Actualizar(Oportunidad modelo)
        {
            _context.Oportunidad.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Oportunidad.FirstOrDefault(o => o.Id == id);
            if (ent == null) return false;
            _context.Oportunidad.Remove(ent);
            return true;
        }

        public Oportunidad? ObtenerPorId(int id)
        {
            return _context.Oportunidad.FirstOrDefault(o => o.Id == id);
        }

        public IQueryable<Oportunidad> ObtenerTodas()
        {
            return _context.Oportunidad.AsQueryable();
        }
        public Oportunidad? ObtenerPorIdConPersona(int id)
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Include(o => o.PotencialCliente)
                .FirstOrDefault(o => o.Id == id);
        }

        public PotencialCliente? ObtenerPersonaPorOportunidad(int idOportunidad)
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Where(o => o.Id == idOportunidad)
                .Select(o => o.PotencialCliente)
                .FirstOrDefault();
        }

        public List<Persona> ObtenerPersonasPorOportunidades(IEnumerable<int> idsOportunidad)
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Where(o => idsOportunidad.Contains(o.Id))
                .Select(o => o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona : null)
                .Where(p => p != null)
                .Distinct()
                .ToList()!;
        }

        public IQueryable<Oportunidad> ObtenerTodosConPersona()
        {
            return _context.Oportunidad
                .AsNoTracking()
                .Include(o => o.PotencialCliente)
                    .ThenInclude(pc => pc.Persona!);
        }

        public IQueryable<Oportunidad> Query()
        {
            return _context.Oportunidad.AsQueryable();
        }

        public IQueryable<Oportunidad> QueryAsNoTracking()
        {
            return _context.Oportunidad.AsNoTracking();
        }
    }
}
