using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;

namespace Olympus.API.Controllers.Configuracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class CFGModUsuariosController : ControllerBase
    {
        private readonly ICFGModUsuariosService _usuarioService;
        private readonly IErrorLogService _errorLogService;

        public CFGModUsuariosController(
            ICFGModUsuariosService usuarioService,
            IErrorLogService errorLogService)
        {
            _usuarioService = usuarioService;
            _errorLogService = errorLogService;
        }

        [HttpPost("RegistrarUsuarioYPersona")]
        public CFGModUsuariosTUsuarioDTORPT RegistrarUsuarioYPersona([FromBody] CFGModUsuariosTUsuarioDTO request)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();
            try
            {
                respuesta = _usuarioService.RegistrarUsuarioYPersona(request);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpPut("EditarUsuarioYPersona/{idUsuario}")]
        public CFGModUsuariosTUsuarioDTORPT EditarUsuarioYPersona(int idUsuario, [FromBody] CFGModUsuariosTUsuarioDTO request)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();
            try
            {
                respuesta = _usuarioService.EditarUsuarioYPersona(request, idUsuario);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpDelete("EliminarUsuarioYPersona/{idUsuario}")]
        public CFGModUsuariosTUsuarioDTORPT EliminarUsuarioYPersona(int idUsuario)
        {
            var respuesta = new CFGModUsuariosTUsuarioDTORPT();
            try
            {
                respuesta = _usuarioService.EliminarUsuarioYPersona(idUsuario);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpGet("ListarConUsuario")]
        public CFGModUsuariosListadoDTORPT ListarConUsuario()
        {
            var respuesta = new CFGModUsuariosListadoDTORPT();
            try
            {
                respuesta = _usuarioService.ListarConUsuario();
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpGet("ListarRoles")]
        public CFGModUsuariosTRolDTORPT ListarRoles()
        {
            var respuesta = new CFGModUsuariosTRolDTORPT();
            try
            {
                respuesta = _usuarioService.ObtenerRoles();
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpGet("ObtenerUsuariosPorRol/{idRol}")]
        public CFGModUsuarioPorRolDTORPT ObtenerUsuariosPorRol(int idRol)
        {
            var respuesta = new CFGModUsuarioPorRolDTORPT();
            try
            {
                respuesta = _usuarioService.ObtenerUsuariosPorRol(idRol);
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
