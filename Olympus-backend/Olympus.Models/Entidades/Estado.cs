using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Estado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public int IdTipo { get; set; }            // FK hacia Tipo
        public int? IdMigracion { get; set; }
        public bool EstadoControl { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Tipo? Tipo { get; set; }
        public List<HistorialEstado> HistorialEstado { get; set; } = new();
        public List<Ocurrencia> Ocurrencias { get; set; } = new();
    }
}
