using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class PotencialCliente
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public bool Desuscrito { get; set; }
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public List<Oportunidad> Oportunidades { get; set; } = new List<Oportunidad>();
        public Persona? Persona { get; set; }   // 1:1

    }
}
