using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Configuracion
{
    public static class SR
    {
        // Mensajes
        public const string _M_MENSAJE_ERROR = "Ocurrió un error al procesar la solicitud. Por favor, inténtelo de nuevo más tarde.";
        public const string _M_CREDENCIALES_INVALIDAS = "Credenciales inválidas.";
        public const string _M_NO_AUTORIZADO = "No tiene permisos para realizar esta acción.";

        // Códigos
        public const string _C_ERROR_CONTROLADO = "ERROR_CONTROLADO";
        public const string _C_ERROR_CRITICO = "ERROR_CRITICO";
        public const string _C_SIN_ERROR = "SIN ERROR";

    }
}
