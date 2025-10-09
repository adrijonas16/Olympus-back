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
        public string Detalle { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public DateTime? FechaRecordatorio { get; set; }
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Oportunidad? Oportunidad { get; set; }
    }
}
