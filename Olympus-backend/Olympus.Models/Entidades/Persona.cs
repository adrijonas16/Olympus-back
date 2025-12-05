namespace Modelos.Entidades
{
    public class Persona
    {
        public int Id { get; set; }

        public int? IdPais { get; set; }
        public string? Nombres { get; set; } = string.Empty;
        public string? Apellidos { get; set; } = string.Empty;
        public string? Celular { get; set; } = string.Empty;
        public string? PrefijoPaisCelular { get; set; } = string.Empty;
        public string? Correo { get; set; } = string.Empty;
        public string? AreaTrabajo { get; set; } = string.Empty;
        public string? Industria { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;
        public int? IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }
        public Docente? Docente { get; set; }
        public PotencialCliente? PotencialCliente { get; set; }
        public Pais? Pais { get; set; }
    }
}
