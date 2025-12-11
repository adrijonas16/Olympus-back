using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class CobranzaPago
    {
        public int Id { get; set; }
        public int IdCobranzaPlan { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public int? IdMetodoPago { get; set; }
        public string? Referencia { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        public CobranzaPlan? Plan { get; set; }
        public ICollection<CobranzaPagoAplicacion>? Aplicaciones { get; set; }
    }
}
