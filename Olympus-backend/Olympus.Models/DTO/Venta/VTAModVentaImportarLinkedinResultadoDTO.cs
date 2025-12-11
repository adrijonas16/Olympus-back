using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{

    public class VTAModVentaImportarLinkedinResultadoDTO
    {
        public CFGRespuestaGenericaDTO Respuesta { get; set; } = new CFGRespuestaGenericaDTO();
        public int FilasProcesadas { get; set; }
        public int FilasEnRango { get; set; }
        public int FilasSaltadas { get; set; }
        public List<VTAModVentaImportarLinkedinSkippedDTO> SkippedSources { get; set; } = new List<VTAModVentaImportarLinkedinSkippedDTO>();
    }

}
