using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaCuotaDTO
    {
        public int Id { get; set; }
        public int IdCobranzaPlan { get; set; }
        public int Numero { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoProgramado { get; set; }
        public decimal MontoPagado { get; set; }
        public decimal Pendiente => MontoProgramado - MontoPagado;
    }
}
