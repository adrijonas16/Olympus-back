using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaOportunidadDetalleDTO
    {
        public int Id { get; set; }
        public int IdPotencialCliente { get; set; }
        public string PersonaNombre { get; set; } = string.Empty;
        public int? IdProducto { get; set; }
        public string ProductoNombre { get; set; } = string.Empty;
        public string PersonaCorreo { get; set; } = string.Empty;
        public string? CodigoLanzamiento { get; set; }
        public int TotalOportunidadesPersona { get; set; }
        public string? Origen { get; set; }
        public int? IdHistorialEstado { get; set; }      // último historialEstado.Id
        public int? IdEstado { get; set; }               // último historialEstado.IdEstado
        public string NombreEstado { get; set; } = string.Empty;
        public int? IdHistorialInteraccion { get; set; } // la interacción tipo 10
        public DateTime? FechaRecordatorio { get; set; } // NULL si la fecha ya paso o no se encontró
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaOportunidadDetalleDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaOportunidadDetalleDTO> Oportunidad { get; set; } = new List<VTAModVentaOportunidadDetalleDTO>();
        public List<VTAModVentaTHistorialEstadoDetalleDTO> HistorialActual { get; set; } = new List<VTAModVentaTHistorialEstadoDetalleDTO>();

    }
}
