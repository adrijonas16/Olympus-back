using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class MetodoPagoProducto
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdMetodoPago { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public Producto? Producto { get; set; }
        public MetodoPago? MetodoPago { get; set; }
    }
}
