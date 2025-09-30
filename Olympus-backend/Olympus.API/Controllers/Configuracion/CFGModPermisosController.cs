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

        public CFGModPermisosController(ICFGModPermisosService areaService, IErrorLogService errorLogService)
        {
            _areaService = areaService;
            _errorLogService = errorLogService;
        }

        /// <summary>
        /// Obtiene todas las áreas activas.
        /// </summary>
        /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
        /// <FechaCreacion>2025-09-02</FechaCreacion>
        [HttpGet("ObtenerTodas")]
        public CFGModPermisosTAreaDTORPT ObtenerTodas()
        {
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
            return respuesta;
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
