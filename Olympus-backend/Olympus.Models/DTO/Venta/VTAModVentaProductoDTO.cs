using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaProductoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoLanzamiento { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaPresentacion { get; set; }
        public string? DatosImportantes { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public decimal? CostoBase { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaProductoDTORPT : CFGRespuestaGenericaDTO {    
        public List<VTAModVentaProductoDTO> Productos { get; set; } = new(); 
    }
}

