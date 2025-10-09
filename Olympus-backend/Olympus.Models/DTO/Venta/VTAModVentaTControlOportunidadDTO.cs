using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTControlOportunidadDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public int? IdMigracion { get; set; }
        public bool Estado { get; set; }
    }
    public class VTAModVentaTControlOportunidadDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTControlOportunidadDTO> ControlOportunidad { get; set; } = new List<VTAModVentaTControlOportunidadDTO>();
    }
}
