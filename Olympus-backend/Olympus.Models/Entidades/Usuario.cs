namespace Modelos.Entidades
{
    public partial class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string Correo { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int IdRol { get; set; }

        public Rol Rol { get; set; } = null!;

        public DateTime FechaCreacion { get; set; }

        public int UsuarioCreacion { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int UsuarioModificacion { get; set; }

        public bool Activo { get; set; }
    }
}
