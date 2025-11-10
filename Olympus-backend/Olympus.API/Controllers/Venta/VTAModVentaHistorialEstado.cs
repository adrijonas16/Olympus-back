using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;

namespace Olympus.API.Controllers.Venta
{
    [Route("api/[controller]")]
    [ApiController]
    public class VTAModVentaHistorialEstadoController : ControllerBase
    {
        private readonly IVTAModVentaHistorialEstadoService _historialEstadoService;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHistorialEstadoController(IVTAModVentaHistorialEstadoService historialEstadoService, IErrorLogService errorLogService)
        {
            _historialEstadoService = historialEstadoService;
            _errorLogService = errorLogService;
        }

        [HttpGet("ObtenerTodas")]
        public VTAModVentaTHistorialEstadoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTHistorialEstadoDTORPT();
            try
            {
                respuesta = _historialEstadoService.ObtenerTodas();
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
        public VTAModVentaTHistorialEstadoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTHistorialEstadoDTO();
            try
            {
                dto = _historialEstadoService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        [HttpPost("Insertar")]
        public CFGRespuestaGenericaDTO Insertar([FromBody] VTAModVentaTHistorialEstadoDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _historialEstadoService.Insertar(request);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpPost("InsertarConTipos")]
        public CFGRespuestaGenericaDTO InsertarConTipos([FromBody] VTAModVentaHistorialEstadoCrearTipoDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _historialEstadoService.InsertarConTipos(request);
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
        public CFGRespuestaGenericaDTO Actualizar([FromBody] VTAModVentaTHistorialEstadoDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _historialEstadoService.Actualizar(request);
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
                respuesta = _historialEstadoService.Eliminar(id);
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
