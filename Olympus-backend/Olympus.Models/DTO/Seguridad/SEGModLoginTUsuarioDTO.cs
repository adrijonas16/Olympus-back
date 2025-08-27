using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Seguridad
{
    public class LoginRequest
    {
        public string Correo { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO: CFGRespuestaGenericaDTO
    {
        public string Token { get; set; }
    }
}
