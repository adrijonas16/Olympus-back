using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTOportunidadDTO
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public string CodigoLanzamiento { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
    public class VTAModVentaTOportunidadDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTOportunidadDTO> Oportunidad { get; set; } = new List<VTAModVentaTOportunidadDTO>();
    }
}
