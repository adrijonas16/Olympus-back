using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Lanzamiento
    {
        public int Id { get; set; }
        public string CodigoLanzamiento { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public List<Oportunidad> Oportunidades { get; set; } = new List<Oportunidad>();
    }
}
