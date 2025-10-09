using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTHistorialEstadoDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdMotivo { get; set; }
        public int? IdEstado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? CantidadLlamadasContestadas { get; set; }
        public int? CantidadLlamadasNoContestadas { get; set; }
        public int? IdMigracion { get; set; }
        public bool Estado { get; set; }
    }
    public class VTAModVentaTHistorialEstadoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTHistorialEstadoDTO> HistorialEstado { get; set; } = new List<VTAModVentaTHistorialEstadoDTO>();
    }
}
