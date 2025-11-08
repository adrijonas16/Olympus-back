using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Certificado
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = string.Empty;
        public string? Codigo { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        public List<ProductoCertificado> ProductoCertificados { get; set; } = new List<ProductoCertificado>();
    }
}
