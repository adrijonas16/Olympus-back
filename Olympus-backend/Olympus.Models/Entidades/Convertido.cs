using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Convertido
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdInversion { get; set; }
        public int? IdProducto { get; set; }
        public bool PagoCompleto { get; set; } = false;
        public decimal? MontoPagado { get; set; }
        public DateTime? FechaPago { get; set; }
        public string? Moneda { get; set; } = string.Empty;
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
