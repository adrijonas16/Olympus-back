using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaOportunidadService
    {
        VTAModVentaTOportunidadDTORPT ObtenerTodas();
        VTAModVentaTOportunidadDTO ObtenerPorId(int id);
        VTAModVentaTOportunidadDTORPT ObtenerPorPersona(int idPersona);
        VTAModVentaTControlOportunidadDTORPT ObtenerControlOportunidadesPorOportunidad(int idOportunidad);
        VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int idOportunidad);
        VTAModVentaTHistorialEstadoDTORPT ObtenerHistorialEstadoPorOportunidad(int idOportunidad);
        VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle();
        VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId(int id);
        VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle_SP_Multi(string? tipoInteraccion = null);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}