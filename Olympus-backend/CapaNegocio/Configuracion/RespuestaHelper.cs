using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;

namespace CapaNegocio.Configuracion
{
    public class RespuestaHelper
    {
        public static CFGRespuestaGenericaDTO Respuesta (string codigo,string mensaje,string detalle)
        {
            return new CFGRespuestaGenericaDTO
            {
                Codigo = codigo,
                Mensaje = mensaje,
            };
        }
    }
}
