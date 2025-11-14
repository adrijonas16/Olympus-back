using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaOportunidadService
    {
        VTAModVentaTOportunidadDTORPT ObtenerTodas();
        VTAModVentaTOportunidadDTO ObtenerPorId(int id);
        VTAModVentaOportunidadDetalleDTORPT ObtenerDetallePorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
        VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int id, int? idTipo = null);
        VTAModVentaTOportunidadDetalleDTORPT ObtenerHistorialEstadoPorOportunidad(int id);
        VTAModVentaOportunidadDetalleDTORPT ObtenerTodasOportunidadesRecordatorio();
        VTAModVentaOportunidadDetalleDTO ObtenerOportunidadPorIdConRecordatorio(int id);
        VTAModVentaPotencialClienteDTO ObtenerPotencialPorOportunidadId(int idOportunidad);


        //VTAModVentaTOportunidadDTORPT ObtenerPorPersona(int idPersona);
        //VTAModVentaTControlOportunidadDTORPT ObtenerControlOportunidadesPorOportunidad(int idOportunidad);
        //VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int idOportunidad);
        //VTAModVentaTHistorialEstadoDTORPT ObtenerHistorialEstadoPorOportunidad(int idOportunidad);
        //VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle();
        //VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId(int id);
        //VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle_SP_Multi(string? tipoInteraccion = null);
        //VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId_SP(int idOportunidad);
    }
}