using CapaDatos.DataContext;
using CapaDatos.Repositorio.Configuracion;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Seguridad
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly OlympusContext _context;
        public UserTokenRepository(OlympusContext context)
        {
            _context = context;
        }

        public bool Actualizar(UserToken modelo)
        {
            _context.UserToken.Update(modelo);
            return true;
        }

        public bool Eliminar(int id)
        {
            var ent = _context.UserToken.FirstOrDefault(x => x.Id == id);
            if (ent == null) return false;
            _context.UserToken.Remove(ent);
            return true;
        }

        public bool Insertar(UserToken modelo)
        {
            _context.UserToken.Add(modelo);
            return true;
        }

        public UserToken? ObtenerPorId(int id)
        {
            return _context.UserToken.FirstOrDefault(t => t.Id == id);
        }

        public IQueryable<UserToken> ObtenerTodos()
        {
            return _context.UserToken.AsQueryable();
        }

        public UserToken? ObtenerPorToken(string token)
        {
            return _context.UserToken.FirstOrDefault(t => t.Token == token);
        }
    }
}
