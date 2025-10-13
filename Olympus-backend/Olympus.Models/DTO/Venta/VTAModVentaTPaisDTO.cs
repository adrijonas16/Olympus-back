using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTPaisDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int PrefijoCelularPais { get; set; }
        public int DigitoMaximo { get; set; }
        public int DigitoMinimo { get; set; }
        public bool Estado { get; set; }
    }

    public class VTAModVentaTPaisDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTPaisDTO> Pais { get; set; } = new List<VTAModVentaTPaisDTO>();
    }
}
