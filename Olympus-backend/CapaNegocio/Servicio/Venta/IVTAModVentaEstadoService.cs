using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaEstadoService
    {
        VTAModVentaTEstadoDTORPT ObtenerTodas();
        VTAModVentaTEstadoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTEstadoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTEstadoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
