using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class HistorialEstado
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdEstado { get; set; }
        public string? Observaciones { get; set; } = string.Empty;
        public int? CantidadLlamadasContestadas { get; set; }
        public int? CantidadLlamadasNoContestadas { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Oportunidad? Oportunidad { get; set; }
        public Asesor? Asesor { get; set; }
        public Estado? EstadoReferencia { get; set; } // sin conflicto con "Estado"
        public List<HistorialEstadoTipo> HistorialEstadoTipos { get; set; } = new List<HistorialEstadoTipo>();
        public List<Cobranza> Cobranzas { get; set; } = new List<Cobranza>();
        public List<Convertido> Convertidos { get; set; } = new List<Convertido>();
        public List<VentaCruzada> VentaCruzadas { get; set; } = new List<VentaCruzada>();
        public List<Corporativo> Corporativos { get; set; } = new List<Corporativo>();
    }
}
