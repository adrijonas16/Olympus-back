using Modelos.DTO.Seguridad;
using System.Runtime.CompilerServices;

namespace CapaNegocio.Servicio.Configuracion
{
    public interface IErrorLogService
    {
        bool RegistrarError(Exception ex,
            [CallerMemberName] string metodo = "",
            [CallerFilePath] string archivo = "",
            [CallerLineNumber] int linea = 0);
    }
}
