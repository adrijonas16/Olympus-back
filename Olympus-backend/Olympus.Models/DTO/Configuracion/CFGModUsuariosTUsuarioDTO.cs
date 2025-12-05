namespace Modelos.DTO.Configuracion
{
    public class CFGModUsuariosTUsuarioDTO
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string CorreoUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public int IdPais { get; set; }
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Celular { get; set; }
        public string? PrefijoPaisCelular { get; set; }
        public string? AreaTrabajo { get; set; }
        public string? Industria { get; set; }
    }

    public class CFGModUsuariosTUsuarioDTORPT : CFGRespuestaGenericaDTO
    {
        public int? IdPersona { get; set; }
        public int? IdUsuario { get; set; }
    }

    public class CFGModUsuariosListadoDTO
    {
        public int IdUsuario { get; set; }        
        public int IdPersona { get; set; } 
        public string Nombres { get; set; } = string.Empty;   
        public string Apellidos { get; set; } = string.Empty; 
        public string Correo { get; set; } = string.Empty;   
        public string Rol { get; set; } = string.Empty;      
        public int IdRol { get; set; }                       
        public bool Activo { get; set; }                     
        public string? Celular { get; set; }                 
        public string? PrefijoPaisCelular { get; set; }      
        public int? IdPais { get; set; }
        public string? Industria { get; set; }               
        public string? AreaTrabajo { get; set; }             
    }


    public class CFGModUsuariosListadoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModUsuariosListadoDTO> Usuarios { get; set; } = new();
    }

    public class CFGModUsuarioPorRolDTO
    {
        public int Id { get; set; } 
        public int IdPersona { get; set; }
        public string Nombre { get; set; } = string.Empty; 
        public int IdRol { get; set; } 
    }

    public class CFGModUsuarioPorRolDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModUsuarioPorRolDTO> Usuarios { get; set; } = new();
    }


}
