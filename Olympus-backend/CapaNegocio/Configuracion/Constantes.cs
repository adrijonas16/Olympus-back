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
        public const string _M_NO_AREAS_PARA_MOSTRAR = "No hay areas que mostrar.";
        public const string _M_NO_MODULOS_PARA_MOSTRAR = "No hay modulos que mostrar.";
        public const string _M_NO_ENCONTRADO = "No hay registros que mostrar.";
        public const string _M_ERROR_TOKEN = "Token inválido, expirado o revocado";

        // Códigos
        public const string _C_ERROR_CONTROLADO = "ERROR_CONTROLADO";
        public const string _C_ERROR_CRITICO = "ERROR_CRITICO";
        public const string _C_SIN_ERROR = "SIN ERROR";
        public const string _C_ERROR_UNAUTHORIZED = "UNAUTHORIZED";

    }
}
