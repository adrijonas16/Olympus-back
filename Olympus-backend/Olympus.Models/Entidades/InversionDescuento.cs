using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class InversionDescuento
    {
        public int Id { get; set; }
        public int IdInversion { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public int? IdTipo { get; set; }
        public decimal? Porcentaje { get; set; }
        public decimal? Monto { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public bool Activo { get; set; } = true;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // navegación
        public Inversion? Inversion { get; set; }
        public Tipo? Tipo { get; set; }
    }
}
