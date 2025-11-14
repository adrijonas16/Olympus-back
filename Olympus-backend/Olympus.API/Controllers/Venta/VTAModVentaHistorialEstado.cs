using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;

namespace Olympus.API.Controllers.Venta
{
    [Route("api/[controller]")]
    [ApiController]
    public class VTAModVentaHistorialEstadoController : ControllerBase
    {
        private readonly IVTAModVentaHistorialEstadoService _historialEstadoService;
        private readonly IVTAModVentaEstadoTransicionService _estadoTransicionService;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHistorialEstadoController(
            IVTAModVentaHistorialEstadoService historialEstadoService,
            IVTAModVentaEstadoTransicionService estadoTransicionService,
            IErrorLogService errorLogService)
        {
            _historialEstadoService = historialEstadoService;
            _estadoTransicionService = estadoTransicionService;
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

        /// Devuelve las ocurrencias disponibles para la oportunidad, con la bandera Allowed/Permitida.
        /// GET /api/VTAModVentaHistorialEstado/{oportunidadId}/ocurrenciasDisponibles
        [HttpGet("{oportunidadId:int}/ocurrenciasDisponibles")]
        public IActionResult ObtenerOcurrenciasDisponibles(int oportunidadId)
        {
            try
            {
                var ocurrencias = _estadoTransicionService.ObtenerOcurrenciasPermitidas(oportunidadId);

                return Ok(new
                {
                    Ocurrencias = ocurrencias,
                    Codigo = SR._C_SIN_ERROR,
                    Mensaje = string.Empty
                });
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new { Codigo = SR._C_ERROR_CRITICO, Mensaje = ex.Message });
            }
        }

        /// Crea un HistorialEstado a partir de la ocurrencia seleccionada.
        /// POST /api/VTAModVentaHistorialEstado/{oportunidadId}/crear-con-ocurrencia
        /// Body: { "ocurrenciaId": 2, "usuario": "j.ramirez" }
        [HttpPost("{oportunidadId:int}/crearConOcurrencia")]
        public IActionResult CrearConOcurrencia(int oportunidadId, [FromBody] CrearHistorialRequest request)
        {
            if (request == null)
                return BadRequest(new CFGRespuestaGenericaDTO { Codigo = SR._C_ERROR_CONTROLADO, Mensaje = "Cuerpo de solicitud vacío" });

            try
            {
                var resp = _estadoTransicionService.CrearHistorialConOcurrencia(oportunidadId, request.OcurrenciaId, request.Usuario);
                if (resp == null)
                    return StatusCode(500, new CFGRespuestaGenericaDTO { Codigo = SR._C_ERROR_CRITICO, Mensaje = "Respuesta nula del servicio" });

                if (resp.Codigo == SR._C_SIN_ERROR)
                    return Ok(resp);

                return BadRequest(resp);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new CFGRespuestaGenericaDTO { Codigo = SR._C_ERROR_CRITICO, Mensaje = ex.Message });
            }
        }

        public class CrearHistorialRequest
        {
            public int OcurrenciaId { get; set; }
            public string Usuario { get; set; } = "SYSTEM";
        }
    }
}
