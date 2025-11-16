using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class OcurrenciaRepository : IOcurrenciaRepository
    {
        private readonly OlympusContext _context;

        public OcurrenciaRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Ocurrencia modelo)
        {
            _context.Ocurrencia.Add(modelo);
            return true;
        }

        public bool Actualizar(Ocurrencia modelo)
        {
            _context.Ocurrencia.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Ocurrencia.FirstOrDefault(o => o.Id == id);
            if (ent == null) return false;
            _context.Ocurrencia.Remove(ent);
            return true;
        }

        public Ocurrencia? ObtenerPorId(int id)
        {
            return _context.Ocurrencia
                .Where(o => o.Id == id)
                .Select(o => new Ocurrencia
                {
                    Id = o.Id,
                    Nombre = o.Nombre,
                    Descripcion = o.Descripcion,
                    IdEstado = o.IdEstado,
                    FechaCreacion = o.FechaCreacion,
                    UsuarioCreacion = o.UsuarioCreacion,

                })
                .FirstOrDefault();
        }


        public IQueryable<Ocurrencia> ObtenerTodos()
        {
            return _context.Ocurrencia.AsNoTracking().AsQueryable();
        }

        public IQueryable<Ocurrencia> Query()
        {
            return _context.Ocurrencia.AsNoTracking().AsQueryable();
        }
    }
}
