using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaTHistorialEstadoDTO
    {
        public int Id { get; set; }
        public int IdOportunidad { get; set; }
        public int? IdAsesor { get; set; }
        public int? IdEstado { get; set; }
        public int? IdOcurrencia { get; set; }
        public string Observaciones { get; set; } = string.Empty;
        public int? CantidadLlamadasContestadas { get; set; }
        public int? CantidadLlamadasNoContestadas { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
    public class VTAModVentaTHistorialEstadoDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaTHistorialEstadoDTO> HistorialEstado { get; set; } = new List<VTAModVentaTHistorialEstadoDTO>();
        public List<VTAModVentaVentaCruzadaDTO> VentaCruzada { get; set; } = new List<VTAModVentaVentaCruzadaDTO>();
        public List<VTAModVentaHistorialEstadoTipoDTO> HistorialEstadoTipos { get; set; } = new List<VTAModVentaHistorialEstadoTipoDTO>();
        public List<VTAModVentaCobranzaDTO> Cobranzas { get; set; } = new List<VTAModVentaCobranzaDTO>();
        public List<VTAModVentaConvertidoDTO> Convertidos { get; set; } = new List<VTAModVentaConvertidoDTO>();
        public List<VTAModVentaVentaCruzadaDTO> VentaCruzadas { get; set; } = new List<VTAModVentaVentaCruzadaDTO>();
        public List<VTAModVentaCorporativoDTO> Corporativos { get; set; } = new List<VTAModVentaCorporativoDTO>();
        public List<VTAModVentaOcurrenciaDTO> Ocurrencias { get; set; } = new List<VTAModVentaOcurrenciaDTO>();
    }
}
