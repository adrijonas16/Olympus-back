using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaMetodoPagoProductoDTO
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdMetodoPago { get; set; }
        public string NombreMetodoPago { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaMetodoPagoProductoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaMetodoPagoProductoDTO> MetodoPagoProductos { get; set; } = new(); 
    }

}
