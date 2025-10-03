using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;

namespace Olympus.API.Controllers.Configuracion
{
    [Route("api/[controller]")]
    [ApiController]
    public class CFGModPermisosController : Controller
    {
        private readonly ICFGModPermisosService _areaService;
        private readonly IErrorLogService _errorLogService;
        private readonly TokenService _tokenService;

        public CFGModPermisosController(ICFGModPermisosService areaService, IErrorLogService errorLogService, TokenService tokenService)
        {
            _areaService = areaService;
            _errorLogService = errorLogService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Obtiene todas las áreas activas.
        /// </summary>
        /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
        /// <FechaCreacion>2025-09-02</FechaCreacion>
        [HttpGet("ObtenerTodas")]
        public IActionResult ObtenerTodas()
        {
            string token = Request.Headers["Authorization"];
            var tokenRenovado = _tokenService.VerificarYRenovarToken(token);

            if (tokenRenovado == null)
            {
                return Unauthorized(new CFGModPermisosTAreaDTORPT
                {
                    Codigo = SR._C_ERROR_UNAUTHORIZED,
                    Mensaje = "Token inválido, expirado o revocado"
                });
            }

            Response.Headers.Add("X-Token-Renewed", tokenRenovado);

            CFGModPermisosTAreaDTORPT respuesta = new CFGModPermisosTAreaDTORPT();
            try
            {
                respuesta = _areaService.ObtenerTodas();
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return Ok(respuesta);
        }

        /// <summary>
        /// Obtiene todas las modulos por areas.
        /// </summary>
        /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
        /// <FechaCreacion>2025-09-02</FechaCreacion>
        [HttpPost("ObtenerPorAreas")]
        public CFGModPermisosTModuloDTORPT ObtenerPorAreas([FromBody] List<int> idsAreas)
        {
            CFGModPermisosTModuloDTORPT respuesta = new CFGModPermisosTModuloDTORPT();
            try
            {
                respuesta = _areaService.ObtenerPorAreas(idsAreas);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// <summary>
        /// Obtiene los formularios por modulo
        /// </summary>
        /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
        /// <FechaCreacion>2025-09-02</FechaCreacion>
        [HttpPost("ObtenerFormularioPorModulo")]
        public CFGModPermisosTFormularioDTORPT ObtenerFormularioPorModulo([FromBody] int idModulo)
        {
            CFGModPermisosTFormularioDTORPT respuesta = new CFGModPermisosTFormularioDTORPT();
            try
            {
                respuesta = _areaService.ObtenerFormularioPorModulo(idModulo);
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
