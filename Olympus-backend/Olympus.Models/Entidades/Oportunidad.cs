using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Oportunidad
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public int IdLanzamiento { get; set; }
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // FKs / Navegación
        public Persona? Persona { get; set; }
        public Lanzamiento? Lanzamiento { get; set; }
        public List<ControlOportunidad> ControlOportunidades { get; set; } = new List<ControlOportunidad>();
        public List<HistorialEstado> HistorialEstado { get; set; } = new List<HistorialEstado>();
        public List<HistorialInteraccion> HistorialInteracciones { get; set; } = new List<HistorialInteraccion>();
    }
}
