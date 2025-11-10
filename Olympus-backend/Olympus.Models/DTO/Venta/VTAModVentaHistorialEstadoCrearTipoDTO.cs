using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaHistorialEstadoCrearTipoDTO
    {
        public int IdOportunidad { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdEstado { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? CantidadLlamadasContestadas { get; set; }
        public int? CantidadLlamadasNoContestadas { get; set; }
        public bool Estado { get; set; } = true;
        public string? UsuarioCreacion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public List<int> Tipos { get; set; } = new();
    }
}
