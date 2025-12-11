using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
        public class VTAModVentaEstadoTransicionDTO
        {
            public int Id { get; set; }
            public int? IdEstadoOrigen { get; set; }
            public int? IdEstadoDestino { get; set; }
            public int? IdOcurrenciaOrigen { get; set; }
            public int? IdOcurrenciaDestino { get; set; }
            public bool Permitido { get; set; } = true;
            public string? Comentario { get; set; } = string.Empty;
            public bool Estado { get; set; } = true;
            public int? IdMigracion { get; set; }
            public DateTime FechaCreacion { get; set; }
            public string UsuarioCreacion { get; set; } = string.Empty;
            public DateTime? FechaModificacion { get; set; }
            public string? UsuarioModificacion { get; set; } = string.Empty;
    }

        public class VTAModVentaEstadoTransicionDTORPT : CFGRespuestaGenericaDTO
        {
            public List<VTAModVentaEstadoTransicionDTO> Transiciones { get; set; } = new List<VTAModVentaEstadoTransicionDTO>();
        }
}