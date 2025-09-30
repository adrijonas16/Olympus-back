using CapaDatos.Repositorio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Modelos.Entidades;
using System.Runtime.CompilerServices;

namespace CapaNegocio.Configuracion
{
    public class ErrorLogService: IErrorLogService
    {
        private readonly IErrorLogRepository _errorLogRepository;

        public ErrorLogService(IErrorLogRepository errorLogRepository)
        {
            _errorLogRepository = errorLogRepository;
        }

        /// <summary>
        /// Registra un error en la base de datos.
        /// </summary>
        public bool RegistrarError(Exception ex,
                    [CallerMemberName] string metodo = "",
                    [CallerFilePath] string archivo = "",
                    [CallerLineNumber] int linea = 0)
        {
            // Extraer solo el nombre del archivo sin la ruta completa
            var nombreArchivo = System.IO.Path.GetFileNameWithoutExtension(archivo);

            var modelo = new ErrorLog
            {
                FechaHora = DateTime.UtcNow,
                Origen = $"{nombreArchivo}.{metodo} (Línea {linea})",
                Mensaje = ex.Message,
                Traza = ex.ToString()
            };

            return _errorLogRepository.Insertar(modelo);
        }

    }
}
