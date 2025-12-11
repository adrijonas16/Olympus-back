using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaPagoRegistroDTO
    {
        public int IdCobranzaPlan { get; set; }
        public int? IdCuotaInicial { get; set; } // id PK
        public decimal MontoPago { get; set; }
        public int? IdMetodoPago { get; set; }
        public DateTime? FechaPago { get; set; }
        public string Usuario { get; set; } = "SYSTEM";
    }
}
