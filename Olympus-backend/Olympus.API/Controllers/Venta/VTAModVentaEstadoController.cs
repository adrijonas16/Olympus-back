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
    public class VTAModVentaEstadoController : ControllerBase
    {
        private readonly IVTAModVentaEstadoService _estadoService;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaEstadoController(IVTAModVentaEstadoService estadoService, IErrorLogService errorLogService)
        {
            _estadoService = estadoService;
            _errorLogService = errorLogService;
        }

        [HttpGet("ObtenerTodas")]
        public VTAModVentaTEstadoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTEstadoDTORPT();
            try
            {
                respuesta = _estadoService.ObtenerTodas();
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
        public VTAModVentaTEstadoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTEstadoDTO();
            try
            {
                dto = _estadoService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        [HttpPost("Insertar")]
        public CFGRespuestaGenericaDTO Insertar([FromBody] VTAModVentaTEstadoDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _estadoService.Insertar(request);
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
        public CFGRespuestaGenericaDTO Actualizar([FromBody] VTAModVentaTEstadoDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _estadoService.Actualizar(request);
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
                respuesta = _estadoService.Eliminar(id);
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
