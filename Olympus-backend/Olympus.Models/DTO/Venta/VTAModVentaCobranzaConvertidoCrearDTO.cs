using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCobranzaConvertidoCrearDTO
    {
        public int IdOportunidad { get; set; }
        public int IdCobranzaPlan { get; set; }
        public decimal Total { get; set; }
        public DateTime? FechaPago { get; set; }
        public int? IdMetodoPago { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
    }
}
