using CapaDatos.Repositorio.Configuracion;
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

        // Guardar cambios
        Task<int> SaveChangesAsync();
    }
}
