using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
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
        /// GET /api/VTAModVentaHistorialEstado/OcurrenciasPermitidas/{IdOportunidad}/{idUsuario}/{idRol}
        [HttpGet("OcurrenciasPermitidas/{IdOportunidad}")]
        public VTAModVentaOcurrenciasPermitidasDTORPT OcurrenciasPermitidas(int IdOportunidad)
        {
            var respuesta = new VTAModVentaOcurrenciasPermitidasDTORPT();
            try
            {
                respuesta = _estadoTransicionService.ObtenerOcurrenciasPermitidas(IdOportunidad);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// Crea un HistorialEstado a partir de la ocurrencia seleccionada.
        /// POST /api/VTAModVentaHistorialEstado/{oportunidadId}/crear-con-ocurrencia
        /// Body: { "ocurrenciaId": 2, "usuario": "j.ramirez" }
        [HttpPost("CrearHistorialConOcurrencia/{IdOportunidad}")]
        public IActionResult CrearHistorialConOcurrencia(int IdOportunidad, [FromBody] VTAModVentaCrearHistorialDTO request)
        {
            try
            {
                var (respuesta, nuevoId) = _estadoTransicionService.CrearHistorialConOcurrencia(IdOportunidad, request.OcurrenciaId, request.Usuario);
                return Ok(new
                {
                    Codigo = respuesta.Codigo,
                    Mensaje = respuesta.Mensaje,
                    NuevoHistorialId = nuevoId
                });
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return Problem(ex.Message);
            }
        }

        [HttpPost("{IdOportunidad}/IncrementarLlamadas")]
        public IActionResult IncrementarLlamadas(int IdOportunidad, [FromBody] VTAModVentaIncrementarLlamadaDTO request)
        {
            try
            {
                if (request == null)
                    return BadRequest("request is required");

                var tipo = (request.Tipo ?? string.Empty).Trim().ToUpper();
                var usuario = string.IsNullOrWhiteSpace(request.Usuario) ? "SYSTEM" : request.Usuario;

                var resp = _historialEstadoService.IncrementarLlamadas(IdOportunidad, tipo, usuario);
                if (resp.Codigo == SR._C_SIN_ERROR)
                    return Ok(resp);
                return BadRequest(resp);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return Problem(detail: ex.Message);
            }
        }

    }
}
