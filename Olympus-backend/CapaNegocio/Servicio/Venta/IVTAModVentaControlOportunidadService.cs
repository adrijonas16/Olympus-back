using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaControlOportunidadService
    {
        VTAModVentaTControlOportunidadDTORPT ObtenerTodas();
        VTAModVentaTControlOportunidadDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTControlOportunidadDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTControlOportunidadDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}