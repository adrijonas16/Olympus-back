using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaInversionDTO
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdOportunidad { get; set; }
        public decimal CostoTotal { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public decimal? DescuentoPorcentaje { get; set; }
        public decimal? DescuentoMonto { get; set; }
        public decimal? CostoOfrecido { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaInversionDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaInversionDTO> Inversiones { get; set; } = new(); 
    }

}
