using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTHistorialEstadoDetalleDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdEstado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? CantidadLlamadasContestadas { get; set; }
        public int? CantidadLlamadasNoContestadas { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        public VTAModVentaTAsesorDTO? Asesor { get; set; }
        public VTAModVentaTEstadoDTO? EstadoReferencia { get; set; }
    }

    public class VTAModVentaTOportunidadDetalleDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTHistorialEstadoDetalleDTO> HistorialActual { get; set; } = new List<VTAModVentaTHistorialEstadoDetalleDTO>();
        public List<VTAModVentaTOportunidadDTO> Oportunidad { get; set; } = new List<VTAModVentaTOportunidadDTO>();
    }
}
