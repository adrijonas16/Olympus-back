using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaHistorialEstadoDetalleDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int IdAsesor { get; set; }
        public string AsesorNombres { get; set; } = string.Empty;
        public string AsesorApellidos { get; set; } = string.Empty;
        public string IdEstado { get; set; } = string.Empty;
        public string EstadoNombre { get; set; } = string.Empty;
        public int CantidadLlamadasNoContestadas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}
