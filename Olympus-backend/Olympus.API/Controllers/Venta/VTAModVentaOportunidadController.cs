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
        [HttpGet("HistorialEstado/PorOportunidad/{id}")]
        public IActionResult ObtenerHistorialEstadoPorOportunidad(int id)
        {
            try
            {
                var rpt = _oportunidadService.ObtenerHistorialEstadoPorOportunidad(id);

                if (rpt == null || rpt.HistorialActual == null || !rpt.HistorialActual.Any())
                    return NotFound(new { idOportunidad = id, historialEstados = new object[0] });

                var salida = new
                {
                    idOportunidad = id,
                    historialEstados = rpt.HistorialActual
                };

                return Ok(salida);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return StatusCode(500, new { idOportunidad = id, historialEstados = new object[0], error = ex.Message });
            }
        }

        /// Lista todas las oportunidades con NombrePais y HistorialInteraccion de IdTipo = 10
        /// GET api/VTAModVentaOportunidad/ObtenerTodasConRecordatorio
        [HttpGet("ObtenerTodasConRecordatorio")]
        public VTAModVentaOportunidadDetalleDTORPT ObtenerTodasConRecordatorio()
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                respuesta = _oportunidadService.ObtenerTodasOportunidadesRecordatorio();
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


        ///// Obtener una oportunidad por id con NombrePais y recordatorio (tipo 10)
        ///// GET api/VTAModVentaOportunidad/ObtenerPorIdConRecordatorio/1
        //[HttpGet("ObtenerPorIdConRecordatorio/{id}")]
        //public VTAModVentaTOportunidadDetalleDTORPT ObtenerPorIdConRecordatorio(int id)
        //{
        //    var dto = new VTAModVentaOportunidadDetalleDTORPT();
        //    try
        //    {
        //        // Supongo que el service expone este método (ajusta nombre si en tu service es otro)
        //        dto = _oportunidadService.ObtenerTodasOportunidadesRecordatorio(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //    }
        //    return dto;
        //}

        ///// <summary>
        ///// Obtiene todas las oportunidades de una persona
        ///// </summary>
        //[HttpGet("ObtenerPorPersona/{idPersona}")]
        //public VTAModVentaTOportunidadDTORPT ObtenerPorPersona(int idPersona)
        //{
        //    var respuesta = new VTAModVentaTOportunidadDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerPorPersona(idPersona);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}

        ///// <summary>
        ///// Obtiene todos los ControlOportunidad asociados a una Oportunidad.
        ///// </summary>
        //[HttpGet("ControlOportunidad/PorOportunidad/{idOportunidad}")]
        //public VTAModVentaTControlOportunidadDTORPT ObtenerControlOportunidadPorOportunidad(int idOportunidad)
        //{
        //    var respuesta = new VTAModVentaTControlOportunidadDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerControlOportunidadesPorOportunidad(idOportunidad);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}

        ///// <summary>
        ///// Obtiene todos los HistorialInteraccion asociados a una Oportunidad.
        ///// </summary>
        //[HttpGet("HistorialInteraccion/PorOportunidad/{idOportunidad}")]
        //public VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionPorOportunidad(int idOportunidad)
        //{
        //    var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerHistorialInteraccionesPorOportunidad(idOportunidad);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}

        ///// <summary>
        ///// Obtiene todos los HistorialEstado de una Oportunidad
        ///// </summary>
        //[HttpGet("HistorialEstado/PorOportunidad/{idOportunidad}")]
        //public VTAModVentaTHistorialEstadoDTORPT ObtenerHistorialEstadoPorOportunidad(int idOportunidad)
        //{
        //    var respuesta = new VTAModVentaTHistorialEstadoDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerHistorialEstadoPorOportunidad(idOportunidad);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}

        ///// <summary>
        ///// Obtiene todas las oportunidades con detalle (persona nombre/apellidos + último historial con asesor/estado/motivo)
        ///// </summary>
        //[HttpGet("ObtenerTodasConDetalle")]
        //public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle()
        //{
        //    var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerTodasConDetalle();
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}

        //[HttpGet("Detalle/PorId/{id}")]
        //public VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId(int id)
        //{
        //    var dto = new VTAModVentaTOportunidadDetalleDTO();
        //    try
        //    {
        //        dto = _oportunidadService.ObtenerDetallePorId(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //    }
        //    return dto;
        //}
        ///// <summary>
        ///// Endpoint que ejecuta la versión con SP
        ///// Query param opcional: tipoInteraccion
        ///// Ejemplo: GET api/VTAModVentaOportunidad/ObtenerTodasConDetalle_SP_Multi?tipoInteraccion=Recordatorio
        ///// </summary>
        //[HttpGet("ObtenerTodasConDetalle_SP_Multi")]
        //public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle_SP_Multi([FromQuery] string? tipoInteraccion = null)
        //{
        //    var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
        //    try
        //    {
        //        respuesta = _oportunidadService.ObtenerTodasConDetalle_SP_Multi(tipoInteraccion);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //        respuesta.Codigo = SR._C_ERROR_CRITICO;
        //        respuesta.Mensaje = ex.Message;
        //    }
        //    return respuesta;
        //}
        ///// <summary>
        ///// Endpoint que ejecuta la versión con SP para obtener detalle por Id (multi resultset)
        ///// Ejemplo: GET api/VTAModVentaOportunidad/Detalle/PorId_SP/123
        ///// </summary>
        //[HttpGet("Detalle/PorId_SP/{id}")]
        //public VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId_SP(int id)
        //{
        //    var dto = new VTAModVentaTOportunidadDetalleDTO();
        //    try
        //    {
        //        dto = _oportunidadService.ObtenerDetallePorId_SP(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _errorLogService.RegistrarError(ex);
        //    }
        //    return dto;
        //}

    }
}