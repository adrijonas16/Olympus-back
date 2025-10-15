using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTLanzamientoDTO
    {
        public int Id { get; set; }
        public string CodigoLanzamiento { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }

    }

    public class VTAModVentaTLanzamientoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTLanzamientoDTO> Lanzamiento { get; set; } = new List<VTAModVentaTLanzamientoDTO>();
    }
}
