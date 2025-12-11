using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaPlanConCuotasDTO
    {
        public VTAModVentaCobranzaPlanDTO? Plan { get; set; }
        public List<VTAModVentaCobranzaCuotaDTO> Cuotas { get; set; } = new List<VTAModVentaCobranzaCuotaDTO>();
    }
}
