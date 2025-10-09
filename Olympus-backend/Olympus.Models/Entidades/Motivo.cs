using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Motivo
    {
        public int Id { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public int? IdMigracion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public List<HistorialEstado> HistorialEstado { get; set; } = new List<HistorialEstado>();
    }
}
