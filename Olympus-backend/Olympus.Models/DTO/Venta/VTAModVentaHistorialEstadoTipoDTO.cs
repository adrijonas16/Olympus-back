using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaHistorialEstadoTipoDTO
    {
        public int Id { get; set; }
        public int IdHistorialEstado { get; set; }
        public int IdTipo { get; set; }
        public string TipoNombre { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaHistorialEstadoTipoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaHistorialEstadoTipoDTO> HistorialEstadoTipos { get; set; } = new(); 
    }

}
