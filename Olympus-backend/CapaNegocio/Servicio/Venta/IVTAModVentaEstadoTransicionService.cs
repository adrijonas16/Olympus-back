using Modelos.DTO.Configuracion;
using System.Collections.Generic;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaEstadoTransicionService
    {
        List<VTAModVentaEstadoTransicionService.OcurrenciaDto> ObtenerOcurrenciasPermitidas(int IdOport);
        CFGRespuestaGenericaDTO CrearHistorialConOcurrencia(int oportunidadId, int ocurrenciaId, string usuario);
    }
}
