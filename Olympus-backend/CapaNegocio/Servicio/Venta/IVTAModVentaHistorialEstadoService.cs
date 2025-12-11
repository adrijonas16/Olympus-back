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
        CFGRespuestaGenericaDTO InsertarConTipos(VTAModVentaHistorialEstadoCrearTipoDTO dto);
        CFGRespuestaGenericaDTO IncrementarLlamadas(int idOportunidad, string tipo, string usuario = "SYSTEM");
        CFGRespuestaGenericaDTO ReiniciarLlamadas(int idOportunidad);
    }
}
