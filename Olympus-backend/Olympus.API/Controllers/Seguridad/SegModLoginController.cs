using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Seguridad;
using Modelos.DTO.Venta;

namespace Olympus.API.Controllers.Configuracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegModLoginController : ControllerBase
    {
        private readonly ISEGModLoginService _loginService;
        private readonly IErrorLogService _errorLogService;

        public SegModLoginController(ISEGModLoginService loginService, IErrorLogService errorLogService)
        {
            _loginService = loginService;
            _errorLogService = errorLogService;
        }

        /// <summary>
        /// Método de Loegueo
        /// </summary>
        /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
        /// <FechaCreacion>2025-08-27</FechaCreacion>
        /// <UsuarioModificacion>Adriana Chipana</UsuarioModificacion>
        /// <FechaModificacion>2025-08-28</FechaModificacion>
        [HttpPost("login")]
        [AllowAnonymous]
        public LoginResponseDTO Login([FromBody] LoginRequest request)
        {
            LoginResponseDTO respuesta = new LoginResponseDTO();
            try
            {
                respuesta = _loginService.Autenticar(request.Correo, request.Password, request.Ip);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }


        [HttpGet("ObtenerPermisosDeOportunidad/{IdOportunidad}/{idUsuario}/{idRol}")]
        public CFGRespuestaGenericaDTO ObtenerPermisosDeOportunidad(int IdOportunidad, int IdUsuario, int IdRol)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _loginService.ObtenerPermisosDeOportunidad(IdOportunidad, IdUsuario, IdRol);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
