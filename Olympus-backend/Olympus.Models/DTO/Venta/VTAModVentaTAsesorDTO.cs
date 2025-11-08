using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTAsesorDTO
    {
        public int Id { get; set; }
        public int? IdPais { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string PrefijoPaisCelular { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string AreaTrabajo { get; set; } = string.Empty;
        public bool? Cesado { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
    public class VTAModVentaTAsesorDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTAsesorDTO> Asesores { get; set; } = new List<VTAModVentaTAsesorDTO>();
    }
}
