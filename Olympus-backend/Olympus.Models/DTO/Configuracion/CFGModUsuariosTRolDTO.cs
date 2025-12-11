using Modelos.DTO.Configuracion;
using System;

namespace CapaNegocio.Configuracion
{
    public class CFGModUsuariosTRolDTO
    {
        public int Id { get; set; }
        public string NombreRol { get; set; }
        public bool Estado { get; set; }
    }

    public class CFGModUsuariosTRolDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModUsuariosTRolDTO> Rol { get; set; }
    }
}
