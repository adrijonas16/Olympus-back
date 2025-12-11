using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaCrearPlanDTO
    {
        public int IdOportunidad { get; set; }
        public decimal Total { get; set; }
        public int NumCuotas { get; set; }
        public DateTime FechaInicio { get; set; }
        public int FrecuenciaDias { get; set; } = 30;
        public string Usuario { get; set; } = "SYSTEM";
    }
}
