using CapaDatos.Repositorio.Configuracion;
using CapaDatos.Repositorio.Seguridad;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Repositorio.UnitOfWork
{
    public interface IUnitOfWork
    {
        // Aquí defines tus repositorios
        IUsuarioRepository UsuarioRepository { get; }
        IErrorLogRepository ErrorLogRepository { get; }
        IAreaRepository AreaRepository { get; }
        IModuloRepository ModuloRepository { get; }
        IFormularioRepository FormularioRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }

        // Guardar cambios
        Task<int> SaveChangesAsync();
    }
}
