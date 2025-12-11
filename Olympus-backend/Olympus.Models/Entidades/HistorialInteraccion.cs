using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class HistorialInteraccion
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int IdTipo { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public string? Celular { get; set; } = string.Empty;
        public DateTime? FechaRecordatorio { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Tipo? Tipo { get; set; }
        public Oportunidad? Oportunidad { get; set; }
    }
}
