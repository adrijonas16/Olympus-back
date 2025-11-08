using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Cobranza
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdInversion { get; set; }
        public int? IdProducto { get; set; }
        public decimal MontoTotal { get; set; } = 0m;
        public int? NumeroCuotas { get; set; }
        public decimal? MontoPorCuota { get; set; }
        public decimal? MontoPagado { get; set; }
        public decimal? MontoRestante { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public HistorialEstado? HistorialEstado { get; set; }
        public Inversion? Inversion { get; set; }
        public Producto? Producto { get; set; }
    }
}
