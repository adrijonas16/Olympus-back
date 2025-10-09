using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaAsesorService
    {
        VTAModVentaTAsesorDTORPT ObtenerTodas();
        VTAModVentaTAsesorDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTAsesorDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTAsesorDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
