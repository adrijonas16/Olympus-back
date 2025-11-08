using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaHistorialInteraccionService
    {
        VTAModVentaTHistorialInteraccionDTORPT ObtenerTodas();
        VTAModVentaTHistorialInteraccionDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTHistorialInteraccionDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTHistorialInteraccionDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
