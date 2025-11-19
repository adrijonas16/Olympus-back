using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class CobranzaCuota
    {
        public int Id { get; set; }
        public int IdCobranzaPlan { get; set; }
        public int Numero { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoProgramado { get; set; }
        public decimal MontoPagado { get; set; } = 0m;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        // navegación
        public CobranzaPlan? Plan { get; set; }
        public ICollection<CobranzaPagoAplicacion>? Aplicaciones { get; set; }
    }
}
