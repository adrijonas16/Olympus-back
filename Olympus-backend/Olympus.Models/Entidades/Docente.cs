using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Docente
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string? TituloProfesional { get; set; } = string.Empty;
        public string? Especialidad { get; set; } = string.Empty;
        public string? Logros { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public Persona? Persona { get; set; }   // 1:1
        public List<ProductoDocente> ProductoDocentes { get; set; } = new List<ProductoDocente>();
    }
}
