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
    public class VTAModVentaInversionController : ControllerBase
    {
        private readonly IVTAModVentaInversionService _inversionService;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaInversionController(IVTAModVentaInversionService inversionService, IErrorLogService errorLogService)
        {
            _inversionService = inversionService;
            _errorLogService = errorLogService;
        }

        [HttpGet("ObtenerTodas")]
        public VTAModVentaInversionDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaInversionDTORPT();
            try
            {
                respuesta = _inversionService.ObtenerTodas();
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
        public VTAModVentaInversionDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaInversionDTO();
            try
            {
                dto = _inversionService.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        [HttpPost("Insertar")]
        public CFGRespuestaGenericaDTO Insertar([FromBody] VTAModVentaInversionDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _inversionService.Insertar(request);
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
        public CFGRespuestaGenericaDTO Actualizar([FromBody] VTAModVentaInversionDTO request)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                respuesta = _inversionService.Actualizar(request);
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
                respuesta = _inversionService.Eliminar(id);
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
        /// Actualiza el CostoOfrecido en adm.Inversion usando el SP.
        /// Recibe VTAModVentaInversionActualizarDTO y devuelve el DTO con la fila actualizada (o valores por defecto en caso de error).
        /// </summary>
        [HttpPost("ActualizarCostoOfrecido")]
        public VTAModVentaInversionActualizarDTO ActualizarCostoOfrecido([FromBody] VTAModVentaInversionActualizarDTO request)
        {
            var result = new VTAModVentaInversionActualizarDTO
            {
                IdProducto = request?.IdProducto ?? 0,
                IdOportunidad = request?.IdOportunidad ?? 0,
                DescuentoPorcentaje = request?.DescuentoPorcentaje ?? 0m,
                UsuarioModificacion = request?.UsuarioModificacion ?? string.Empty,
                Id = null,
                CostoTotal = null,
                Moneda = string.Empty,
                CostoOfrecido = null,
                DescuentoAplicado = null,
                FechaModificacion = null,
                UsuarioModificacionSalida = string.Empty
            };

            try
            {
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                if (string.IsNullOrWhiteSpace(request.UsuarioModificacion))
                {
                    throw new ArgumentException("UsuarioModificacion no puede ser nulo o vacío.", nameof(request.UsuarioModificacion));
                }

                result = _inversionService.ActualizarCostoOfrecido(request) ?? result;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }

            return result;
        }
    }
}
