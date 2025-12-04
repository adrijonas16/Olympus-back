using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class EstructuraCurricularModulo
    {
        public int Id { get; set; }
        public int IdEstructuraCurricular { get; set; }
        public int IdModulo { get; set; }
        public int? Orden { get; set; }
        public int? Sesiones { get; set; }
        public int? DuracionHoras { get; set; }
        public string? Observaciones { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;

        public int? IdDocente { get; set; }
        public Docente? Docente { get; set; }

        // Navegaciones
        public EstructuraCurricular? EstructuraCurricular { get; set; }
        public Modulo? Modulo { get; set; }
        public List<Horario> Horarios { get; set; } = new List<Horario>();

    }
}
