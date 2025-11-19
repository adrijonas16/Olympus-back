using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class CobranzaPagoAplicacion
    {
        public int Id { get; set; }
        public int IdPago { get; set; }
        public int IdCuota { get; set; }
        public decimal MontoAplicado { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

        public CobranzaPago? Pago { get; set; }
        public CobranzaCuota? Cuota { get; set; }
    }
}
