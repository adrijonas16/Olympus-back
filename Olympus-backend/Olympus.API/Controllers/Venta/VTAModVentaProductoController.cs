using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

[Route("api/[controller]")]
[ApiController]
public class VTAModVentaProductoController : ControllerBase
{
    private readonly IVTAModVentaProductoService _productoService;
    private readonly IErrorLogService _errorLogService;

    public VTAModVentaProductoController(IVTAModVentaProductoService productoService, IErrorLogService errorLogService)
    {
        _productoService = productoService;
        _errorLogService = errorLogService;
    }

    [HttpGet("ObtenerTodas")]
    public VTAModVentaProductoDTORPT ObtenerTodas()
    {
        var respuesta = new VTAModVentaProductoDTORPT();
        try
        {
            respuesta = _productoService.ObtenerTodas();
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
    public VTAModVentaProductoDTO ObtenerPorId(int id)
    {
        var dto = new VTAModVentaProductoDTO();
        try
        {
            dto = _productoService.ObtenerPorId(id);
        }
        catch (Exception ex)
        {
            _errorLogService.RegistrarError(ex);
        }
        return dto;
    }

    [HttpPost("Insertar")]
    public CFGRespuestaGenericaDTO Insertar([FromBody] VTAModVentaProductoDTO request)
    {
        var respuesta = new CFGRespuestaGenericaDTO();
        try
        {
            respuesta = _productoService.Insertar(request);
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
    public CFGRespuestaGenericaDTO Actualizar([FromBody] VTAModVentaProductoDTO request)
    {
        var respuesta = new CFGRespuestaGenericaDTO();
        try
        {
            respuesta = _productoService.Actualizar(request);
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
            respuesta = _productoService.Eliminar(id);
        }
        catch (Exception ex)
        {
            _errorLogService.RegistrarError(ex);
            respuesta.Codigo = SR._C_ERROR_CRITICO;
            respuesta.Mensaje = ex.Message;
        }
        return respuesta;
    }

    [HttpGet("DetallePorOportunidad/{id}")]
    public VTAModVentaProductoDetalleRPT DetallePorOportunidad(int id)
    {
        try
        {
            return _productoService.ObtenerDetallePorOportunidad(id);
        }
        catch (Exception ex)
        {
            _errorLogService.RegistrarError(ex);
            return new VTAModVentaProductoDetalleRPT
            {
                Codigo = SR._C_ERROR_CRITICO,
                Mensaje = ex.Message
            };
        }
    }
}
