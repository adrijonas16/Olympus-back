using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

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

        [HttpGet("ObtenerDetallePorId/{id}")]
        public VTAModVentaOportunidadDetalleDTORPT ObtenerDetallePorId(int id)
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerDetallePorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// GET /api/VTAModVentaOportunidad/ObtenerHistorialInteraccionesOportunidad/1?idTipo=8
        [HttpGet("ObtenerHistorialInteraccionesOportunidad/{id}")]
        public IActionResult ObtenerHistorialInteraccionesOportunidad(int id, [FromQuery] int? idTipo = null)
        {
            try
            {
                var rpt = _oportunidadService.ObtenerHistorialInteraccionesPorOportunidad(id, idTipo);

                if (rpt == null)
                    return NotFound(new { idOportunidad = id, historialInteraciones = new object[0] });

                var salida = new
                {
                    idOportunidad = id,
                    historialInteraciones = rpt.HistorialInteracciones
                };

                return Ok(salida);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new { idOportunidad = id, historialInteraciones = new object[0], error = ex.Message });
            }
        }

        /// Obtener todos los HistorialEstado de una Oportunidad
        /// GET api/VTAModVentaOportunidad/HistorialEstado/PorOportunidad/1
        /// 
        [HttpGet("HistorialEstado/PorOportunidad/{IdOportunidad}")]
        public IActionResult ObtenerHistorialEstadoPorOportunidad(int IdOportunidad)
        {
            try
            {
                var rpt = _oportunidadService.ObtenerHistorialEstadoPorOportunidad(IdOportunidad);

                if (rpt == null || rpt.HistorialActual == null || !rpt.HistorialActual.Any())
                    return NotFound(new { idOportunidad = IdOportunidad, historialEstados = new object[0] });

                var salida = new
                {
                    idOportunidad = IdOportunidad,
                    historialEstados = rpt.HistorialActual
                };

                return Ok(salida);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new { idOportunidad = IdOportunidad, historialEstados = new object[0], error = ex.Message });
            }
        }

        /// Lista todas las oportunidades con NombrePais y HistorialInteraccion de IdTipo = 10
        /// GET api/VTAModVentaOportunidad/ObtenerTodasConRecordatorio
        [HttpGet("ObtenerTodasConRecordatorio")]
        public VTAModVentaOportunidadDetalleDTORPT ObtenerTodasConRecordatorio(
            [FromQuery] int idUsuario,
            [FromQuery] int idRol)
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerTodasOportunidadesRecordatorio2(idUsuario, idRol);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        /// GET api/VTAModVentaOportunidad/ObtenerPotencialPorOportunidad/5
        [HttpGet("ObtenerPotencialPorOportunidad/{IdOportunidad}")]
        public IActionResult ObtenerPotencialPorOportunidad(int IdOportunidad)
        {
            try
            {
                var dto = _oportunidadService.ObtenerPotencialPorOportunidadId(IdOportunidad);

                if (dto == null || dto.Id == 0)
                    return NotFound(new { idOportunidad = IdOportunidad, potencial = new object[0] });

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new { idOportunidad = IdOportunidad, potencial = new object[0], error = ex.Message });
            }
        }

        [HttpPost("InsertarOportunidadHistorialRegistrado")]
        public CFGRespuestaGenericaDTO InsertarOportunidadHistorialRegistrado([FromBody] VTAModVentaTOportunidadDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _oportunidadService.InsertarOportunidadHistorialRegistrado(request);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        [HttpPost("AsignarAsesor")]
        public CFGRespuestaGenericaDTO AsignarAsesor([FromBody] VTAModVentaAsignarAsesorDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _oportunidadService.AsignarAsesor(dto);
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