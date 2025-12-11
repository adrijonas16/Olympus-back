using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaConvertidoDTO
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdInversion { get; set; }
        public int? IdProducto { get; set; }
        public bool PagoCompleto { get; set; }
        public decimal? MontoPagado { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? Moneda { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaConvertidoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaConvertidoDTO> Convertidos { get; set; } = new(); 
    }

}
