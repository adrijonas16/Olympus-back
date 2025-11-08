using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaVentaCruzadaDTO
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdProductoOrigen { get; set; }
        public string ProductoOrigenNombre { get; set; } = string.Empty;
        public int? IdProductoDestino { get; set; }
        public string ProductoDestinoNombre { get; set; } = string.Empty;
        public int? IdFase { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaVentaCruzadaDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaVentaCruzadaDTO> VentaCruzadas { get; set; } = new(); 
    }

}
