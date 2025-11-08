using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class HistorialEstadoTipo
    {
        public int Id { get; set; }
        public int IdHistorialEstado { get; set; }
        public int IdTipo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public HistorialEstado? HistorialEstado { get; set; }
        public Tipo? Tipo { get; set; }
    }
}
