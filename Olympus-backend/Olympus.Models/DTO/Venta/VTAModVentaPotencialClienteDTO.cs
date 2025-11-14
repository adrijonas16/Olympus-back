using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaPotencialClienteDTO
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public bool Desuscrito { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        public VTAModVentaTPersonaDTO? Persona { get; set; }

    }

    public class PotencialClienteDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaPotencialClienteDTO> PotencialClientes { get; set; } = new List<VTAModVentaPotencialClienteDTO>();
    }
}
