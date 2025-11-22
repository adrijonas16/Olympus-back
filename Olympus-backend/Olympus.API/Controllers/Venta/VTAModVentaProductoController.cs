using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
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
