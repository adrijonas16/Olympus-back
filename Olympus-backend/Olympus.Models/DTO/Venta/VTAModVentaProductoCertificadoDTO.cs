using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaProductoCertificadoDTO
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdCertificado { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
    public class VTAModVentaProductoCertificadoDTORPT : CFGRespuestaGenericaDTO { 
        public List<VTAModVentaProductoCertificadoDTO> ProductoCertificados { get; set; } = new(); 
    }

}
