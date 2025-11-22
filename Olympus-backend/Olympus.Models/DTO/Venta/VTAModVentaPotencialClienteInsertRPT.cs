using Modelos.DTO.Configuracion;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaPotencialClienteInsertRPT : CFGRespuestaGenericaDTO
    {
        public int IdPersona { get; set; }
        public int IdPotencialCliente { get; set; }
    }
}

