using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicios
{
    public interface IUsuariosService
    {
        Task<string?> Autenticar(string correo, string password);
    }
}
