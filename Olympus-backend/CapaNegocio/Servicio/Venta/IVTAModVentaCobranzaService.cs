using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaCobranzaService
    {
        int CrearPlanCobranza(VTAModVentaCobranzaCrearPlanDTO dto);
        int RegistrarPago(VTAModVentaCobranzaPagoRegistroDTO dto, bool usarAcumulada = false);
        IEnumerable<VTAModVentaCobranzaCuotaDTO> ObtenerCuotasPorPlan(int idPlans);
        VTAModVentaCobranzaPlanConCuotasDTO? ObtenerPlanPorOportunidad(int idOportunidad);
        VTAModVentaCobranzaConvertidoResultadoDTO CrearConvertidoDirectoYRegistrarPagoConDetalle(VTAModVentaCobranzaConvertidoCrearDTO dto);
        VTAModVentaCobranzaConvertidoResultadoDTO CrearConvertidoDirectoPlan(VTAModVentaCobranzaConvertidoCrearDTO dto);
    }
}
