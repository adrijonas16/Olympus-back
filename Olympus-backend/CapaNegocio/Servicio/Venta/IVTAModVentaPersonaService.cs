using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaPersonaService
    {
        VTAModVentaTPersonaDTORPT ObtenerTodas();
        VTAModVentaTPersonaDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTPersonaDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTPersonaDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
