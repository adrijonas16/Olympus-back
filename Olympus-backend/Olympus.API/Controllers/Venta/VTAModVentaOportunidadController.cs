using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace Olympus.API.Controllers.Venta
{
    [Route("api/[controller]")]
    [ApiController]
    public class VTAModVentaOportunidadController : ControllerBase
    {
        private readonly IVTAModVentaOportunidadService _oportunidadService;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaOportunidadController(IVTAModVentaOportunidadService oportunidadService, IErrorLogService errorLogService)
        {
            _oportunidadService = oportunidadService;
            _errorLogService = errorLogService;
        }

        [HttpGet("ObtenerTodas")]
        public VTAModVentaTOportunidadDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerTodas();
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpGet("ObtenerPorId/{id}")]
        public VTAModVentaTOportunidadDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTOportunidadDTO();
            try
            {
                dto = _oportunidadService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        [HttpPost("Insertar")]
        public CFGRespuestaGenericaDTO Insertar([FromBody] VTAModVentaTOportunidadDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _oportunidadService.Insertar(request);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpPut("Actualizar")]
        public CFGRespuestaGenericaDTO Actualizar([FromBody] VTAModVentaTOportunidadDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _oportunidadService.Actualizar(request);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpDelete("Eliminar/{id}")]
        public CFGRespuestaGenericaDTO Eliminar(int id)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _oportunidadService.Eliminar(id);
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
        /// Obtiene todas las oportunidades de una persona
        /// </summary>
        [HttpGet("ObtenerPorPersona/{idPersona}")]
        public VTAModVentaTOportunidadDTORPT ObtenerPorPersona(int idPersona)
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerPorPersona(idPersona);
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
        /// Obtiene todos los ControlOportunidad asociados a una Oportunidad.
        /// </summary>
        [HttpGet("ControlOportunidad/PorOportunidad/{idOportunidad}")]
        public VTAModVentaTControlOportunidadDTORPT ObtenerControlOportunidadPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTControlOportunidadDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerControlOportunidadesPorOportunidad(idOportunidad);
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
        /// Obtiene todos los HistorialInteraccion asociados a una Oportunidad.
        /// </summary>
        [HttpGet("HistorialInteraccion/PorOportunidad/{idOportunidad}")]
        public VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerHistorialInteraccionesPorOportunidad(idOportunidad);
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
        /// Obtiene todos los HistorialEstado de una Oportunidad
        /// </summary>
        [HttpGet("HistorialEstado/PorOportunidad/{idOportunidad}")]
        public VTAModVentaTHistorialEstadoDTORPT ObtenerHistorialEstadoPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTHistorialEstadoDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerHistorialEstadoPorOportunidad(idOportunidad);
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
        /// Obtiene todas las oportunidades con detalle (persona nombre/apellidos + último historial con asesor/estado/motivo)
        /// </summary>
        [HttpGet("ObtenerTodasConDetalle")]
        public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle()
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerTodasConDetalle();
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpGet("Detalle/PorId/{id}")]
        public VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId(int id)
        {
            var dto = new VTAModVentaTOportunidadDetalleDTO();
            try
            {
                dto = _oportunidadService.ObtenerDetallePorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }
        /// <summary>
        /// Endpoint que ejecuta la versión con SP
        /// Query param opcional: tipoInteraccion
        /// Ejemplo: GET api/VTAModVentaOportunidad/ObtenerTodasConDetalle_SP_Multi?tipoInteraccion=Recordatorio
        /// </summary>
        [HttpGet("ObtenerTodasConDetalle_SP_Multi")]
        public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle_SP_Multi([FromQuery] string? tipoInteraccion = null)
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerTodasConDetalle_SP_Multi(tipoInteraccion);
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