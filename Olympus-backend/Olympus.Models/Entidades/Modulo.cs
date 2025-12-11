using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Modulo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; }
        public string? Descripcion { get; set; }
        public int? DuracionHoras { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public ICollection<EstructuraCurricularModulo> EstructurasModulos { get; set; } = new List<EstructuraCurricularModulo>();
    }
}
