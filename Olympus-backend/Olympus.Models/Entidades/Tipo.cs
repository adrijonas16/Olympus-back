using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Tipo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;

        // Navegaciones
        public List<HistorialInteraccion> HistorialInteracciones { get; set; } = new List<HistorialInteraccion>();
        public List<Estado> Estados { get; set; } = new List<Estado>();
        public List<HistorialEstadoTipo> HistorialEstadoTipos { get; set; } = new List<HistorialEstadoTipo>();
        public List<InversionDescuento> InversionDescuentos { get; set; } = new List<InversionDescuento>();

    }
}
