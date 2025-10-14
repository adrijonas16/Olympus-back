using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaLanzamientoService
    {
        VTAModVentaTLanzamientoDTORPT ObtenerTodas();
        VTAModVentaTLanzamientoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTLanzamientoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTLanzamientoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id); 
    }
}
