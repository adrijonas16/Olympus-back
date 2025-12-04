using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class EstructuraCurricular
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public Producto? Producto { get; set; }
        public ICollection<EstructuraCurricularModulo> EstructuraCurricularModulos { get; set; } = new List<EstructuraCurricularModulo>();
    }
}
