using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaOcurrenciaSimpleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int? IdEstado { get; set; }
        public bool Allowed { get; set; }
    }

    public class VTAModVentaOcurrenciasPermitidasDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaOcurrenciaSimpleDTO> Ocurrencias { get; set; } = new List<VTAModVentaOcurrenciaSimpleDTO>();
    }
}
