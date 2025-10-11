using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTHistorialInteraccionDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public DateTime? FechaRecordatorio { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? IdMigracion { get; set; }
        public bool Estado { get; set; }
    }
    public class VTAModVentaTHistorialInteraccionDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTHistorialInteraccionDTO> HistorialInteraccion { get; set; } = new List<VTAModVentaTHistorialInteraccionDTO>();
    }
}
