using CapaNegocio.Servicio.Venta;
using Microsoft.AspNetCore.Mvc;
using Modelos.DTO.Venta;
using Modelos.Entidades;

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

    [HttpGet("Plan/PorOportunidad/{idOportunidad}")]
    public IActionResult ObtenerPlanPorOportunidad(int idOportunidad)
    {
        try
        {
            var plan = _cobranzaService.ObtenerPlanPorOportunidad(idOportunidad);
            if (plan == null)
                return NotFound(new { Codigo = "NOT_FOUND", Mensaje = $"No existe plan para la oportunidad {idOportunidad}" });

            return Ok(new { Codigo = "OK", Plan = plan });
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Error al obtener plan por oportunidad", statusCode: 500);
        }
    }

    /// <summary>
    /// Crea un plan de cobranza de 1 cuota para convertido
    /// </summary>
    [HttpPost("ConvertidoDirecto/Plan")]
    public ActionResult<VTAModVentaCobranzaConvertidoResultadoDTO> CrearConvertidoDirectoPlan([FromBody] VTAModVentaCobranzaConvertidoCrearDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var resultado = _cobranzaService.CrearConvertidoDirectoPlan(dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Error al crear plan convertido directo", statusCode: 500);
        }
    }

    /// <summary>
    /// Crea un plan de cobranza y registra el pago de esa cuota
    /// </summary>
    [HttpPost("ConvertidoDirecto/ConDetalle")]
    public ActionResult<VTAModVentaCobranzaConvertidoResultadoDTO> CrearConvertidoDirectoConDetalle([FromBody] VTAModVentaCobranzaConvertidoCrearDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var resultado = _cobranzaService.CrearConvertidoDirectoYRegistrarPagoConDetalle(dto);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, title: "Error al procesar convertido directo", statusCode: 500);
        }
    }

}
