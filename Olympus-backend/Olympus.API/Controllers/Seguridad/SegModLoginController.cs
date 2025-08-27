using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Seguridad;

namespace Olympus.API.Controllers.Configuracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegModLoginController : ControllerBase
    {
        private readonly ISEGModLoginService _loginService;

        public SegModLoginController(ISEGModLoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public LoginResponseDTO Login([FromBody] LoginRequest request)
        {
            LoginResponseDTO respuesta = new LoginResponseDTO();
            try
            {
                respuesta = _loginService.Autenticar(request.Correo, request.Password);
            }
            catch (Exception ex)
            {
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

    }
}
