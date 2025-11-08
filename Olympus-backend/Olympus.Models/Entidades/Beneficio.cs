using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Beneficio
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public int? Orden { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Producto? Producto { get; set; }
    }
}
