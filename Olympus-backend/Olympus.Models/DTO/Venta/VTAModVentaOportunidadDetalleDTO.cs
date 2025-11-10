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
        public int IdPersona { get; set; }
        public string NombrePersona { get; set; } = string.Empty;
        public int? IdPais { get; set; }
        public string NombrePais { get; set; } = string.Empty;
        public int? IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string? CodigoLanzamiento { get; set; }

        public int? IdHistorialEstado { get; set; }      // último historialEstado.Id
        public int? IdEstado { get; set; }               // último historialEstado.IdEstado
        public string NombreEstado { get; set; } = string.Empty;

        public int? IdHistorialInteraccion { get; set; } // la interacción tipo 10
        public DateTime? FechaRecordatorio { get; set; } // NULL si la fecha ya pasó o no existe

        public bool Estado { get; set; }
    }
    public class VTAModVentaOportunidadDetalleDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaOportunidadDetalleDTO> Oportunidad { get; set; } = new List<VTAModVentaOportunidadDetalleDTO>();
    }
}
