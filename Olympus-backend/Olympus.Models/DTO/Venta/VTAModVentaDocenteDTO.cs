using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaDocenteDTO
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string PersonaNombre { get; set; } = string.Empty;
        public string? TituloProfesional { get; set; } = string.Empty;
        public string? Especialidad { get; set; } = string.Empty;
        public string? Logros { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaDocenteDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaDocenteDTO> Docentes { get; set; } = new(); 
    }

}
