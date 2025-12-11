using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.Seguridad
{
    public interface IUserTokenRepository
    {
        bool Insertar(UserToken modelo);
        bool Actualizar(UserToken modelo);
        bool Eliminar(int id);
        UserToken? ObtenerPorId(int id);
        IQueryable<UserToken> ObtenerTodos();
        UserToken? ObtenerPorToken(string token);
    }
}
