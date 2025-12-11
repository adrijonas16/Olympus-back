using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaInversionDescuentoDTO
    {
        public int Id { get; set; }
        public int IdInversion { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public int? IdTipo { get; set; }
        public string TipoNombre { get; set; } = string.Empty;
        public decimal? Porcentaje { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaInversionDescuentoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaInversionDescuentoDTO> InversionDescuentos { get; set; } = new(); 
    }

}
