using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaMotivoService
    {
        VTAModVentaTMotivoDTORPT ObtenerTodas();
        VTAModVentaTMotivoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTMotivoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTMotivoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}