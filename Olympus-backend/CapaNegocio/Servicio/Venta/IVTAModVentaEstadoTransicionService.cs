using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System.Collections.Generic;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaEstadoTransicionService
    {
        VTAModVentaOcurrenciasPermitidasDTORPT ObtenerOcurrenciasPermitidas(int oportunidadId);
        (CFGRespuestaGenericaDTO Respuesta, int NuevoHistorialId) CrearHistorialConOcurrencia(int oportunidadId, int ocurrenciaId, string usuario);
    }
}
