using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTMotivoDTO
    {
        public int Id { get; set; }
        public string Detalle { get; set; } = string.Empty;
        public int? IdMigracion { get; set; }
        public bool Estado { get; set; }
    }
    public class VTAModVentaTMotivoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTMotivoDTO> Motivo { get; set; } = new List<VTAModVentaTMotivoDTO>();
    }
}
