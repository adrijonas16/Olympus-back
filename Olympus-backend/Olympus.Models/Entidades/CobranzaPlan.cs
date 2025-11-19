using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class CobranzaPlan
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int? IdProducto { get; set; }
        public decimal Total { get; set; }
        public int NumCuotas { get; set; }
        public int FrecuenciaDias { get; set; } = 30;
        public DateTime FechaInicio { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        // navegación
        public ICollection<CobranzaCuota>? Cuotas { get; set; }
        public ICollection<CobranzaPago>? Pagos { get; set; }
    }
}
