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
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}