using CapaDatos.DataContext;
using Microsoft.EntityFrameworkCore;
using Modelos.Entidades;
using System.Linq;

namespace CapaDatos.Repositorio.Venta
{
    public class CertificadoRepository : ICertificadoRepository
    {
        private readonly OlympusContext _context;

        public CertificadoRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Insertar(Certificado modelo)
        {
            _context.Certificado.Add(modelo);
            return true;
        }

        public bool Actualizar(Certificado modelo)
        {
            _context.Certificado.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.Certificado.FirstOrDefault(c => c.Id == id);
            if (ent == null) return false;
            _context.Certificado.Remove(ent);
            return true;
        }

        public Certificado? ObtenerPorId(int id)
        {
            return _context.Certificado
                .Include(c => c.ProductoCertificados)
                .FirstOrDefault(c => c.Id == id);
        }

        public IQueryable<Certificado> ObtenerTodos()
        {
            return _context.Certificado.AsNoTracking().AsQueryable();
        }

        public IQueryable<Certificado> Query()
        {
            return _context.Certificado.AsNoTracking().AsQueryable();
        }
    }
}
