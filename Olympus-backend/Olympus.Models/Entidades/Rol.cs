using System;
using System.Collections.Generic;

namespace Modelos.Entidades
{
    public class Rol
    {
        public int Id { get; set; }

        public string NombreRol { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}
