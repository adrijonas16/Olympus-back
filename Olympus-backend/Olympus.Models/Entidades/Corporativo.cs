using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Corporativo
    {
        public int Id { get; set; }
        public int? IdHistorialEstado { get; set; }
        public int? IdProducto { get; set; }
        public int? IdFase { get; set; }
        public int? IdEmpresa { get; set; }
        public int? Cantidad { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public HistorialEstado? HistorialEstado { get; set; }
        public Producto? Producto { get; set; }
    }
}
