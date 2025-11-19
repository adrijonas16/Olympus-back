using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Venta;

[ApiController]
[Route("api/Cobranza")]
public class CobranzaController : ControllerBase
{
    private readonly IVTAModVentaCobranzaService _cobranzaService;
    public CobranzaController(IVTAModVentaCobranzaService cobranzaService) => _cobranzaService = cobranzaService;

    [HttpPost("Plan")]
    public IActionResult CrearPlan([FromBody] VTAModVentaCobranzaCrearPlanDTO dto)
    {
        try
        {
            var idPlan = _cobranzaService.CrearPlanCobranza(dto);
            return Ok(new { Codigo = "OK", NewPlanId = idPlan });
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("Pago")]
    public IActionResult RegistrarPago([FromBody] VTAModVentaCobranzaPagoRegistroDTO dto, [FromQuery] bool acumulada = false)
    {
        try
        {
            var pagoId = _cobranzaService.RegistrarPago(dto, acumulada);
            return Ok(new { Codigo = "OK", PagoId = pagoId });
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet("Plan/{idPlan}/Cuotas")]
    public IActionResult GetCuotas(int idPlan)
    {
        var cuotas = _cobranzaService.ObtenerCuotasPorPlan(idPlan);
        return Ok(new { Codigo = "OK", Cuotas = cuotas });
    }
}
