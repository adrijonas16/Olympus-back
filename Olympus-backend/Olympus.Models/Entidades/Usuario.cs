using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Rol { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public bool Activo { get; set; }
    }
}
