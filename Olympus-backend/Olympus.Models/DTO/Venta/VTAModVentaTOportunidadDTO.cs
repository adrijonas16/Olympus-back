using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTOportunidadDTO
    {
        public int Id { get; set; }
        public int IdPotencialCliente { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdPersona { get; set; }
        public string PersonaNombre { get; set; } = string.Empty;
        public int? IdProducto { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string? CodigoLanzamiento { get; set; }
        public string? Origen { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaRecordatorio { get; set; }
        public string HoraRecordatorio { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        
        public VTAModVentaTHistorialEstadoDetalleDTO? UltimoHistorial { get; set; }
    }
    public class VTAModVentaTOportunidadDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTOportunidadDTO> Oportunidad { get; set; } = new List<VTAModVentaTOportunidadDTO>();
        public List<VTAModVentaTHistorialEstadoDetalleDTO> HistorialActual { get; set; } = new List<VTAModVentaTHistorialEstadoDetalleDTO>();
    }
}
