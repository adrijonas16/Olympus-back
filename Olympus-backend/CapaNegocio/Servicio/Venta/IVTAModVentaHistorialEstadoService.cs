using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaHistorialEstadoService
    {
        VTAModVentaTHistorialEstadoDTORPT ObtenerTodas();
        VTAModVentaTHistorialEstadoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTHistorialEstadoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTHistorialEstadoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
