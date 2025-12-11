using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Horario
    {
        public int Id { get; set; }
        public string Dia { get; set; } = string.Empty;
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public string? Detalle { get; set; } = string.Empty;
        public int? Orden { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        public int? IdEstructuraCurricularModulo { get; set; }
        public EstructuraCurricularModulo? EstructuraCurricularModulo { get; set; }
    }
}

