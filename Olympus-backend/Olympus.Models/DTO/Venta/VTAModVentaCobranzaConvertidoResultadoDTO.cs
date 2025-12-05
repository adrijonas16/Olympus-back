using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaConvertidoResultadoDTO
    {
        public int PlanId { get; set; }
        public int CuotaId { get; set; }
        public int PagoId { get; set; }
        public VTAModVentaCobranzaPlanConCuotasDTO? PlanConCuotas { get; set; }
    }
}
