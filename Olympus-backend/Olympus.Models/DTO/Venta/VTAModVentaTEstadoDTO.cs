using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTEstadoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        public int IdTipo { get; set; }
        public string TipoNombre { get; set; } = string.Empty; 
        public string? TipoCategoria { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaTEstadoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTEstadoDTO> Estados { get; set; } = new List<VTAModVentaTEstadoDTO>();
    }
}
