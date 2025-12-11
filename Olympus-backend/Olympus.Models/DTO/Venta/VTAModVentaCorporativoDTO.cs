using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaCorporativoDTO
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdProducto { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public int? IdFase { get; set; }
        public int? IdEmpresa { get; set; }
        public int? Cantidad { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaCorporativoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaCorporativoDTO> Corporativos { get; set; } = new(); 
    }

}
