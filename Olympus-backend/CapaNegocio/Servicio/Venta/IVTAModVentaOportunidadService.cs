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
        VTAModVentaOportunidadDetalleDTORPT ObtenerTodasOportunidadesRecordatorio2(int idUsuario, int idRol);
        VTAModVentaOportunidadDetalleDTO ObtenerOportunidadPorIdConRecordatorio(int id);
        VTAModVentaPotencialClienteDTO ObtenerPotencialPorOportunidadId(int idOportunidad);
        CFGRespuestaGenericaDTO InsertarOportunidadHistorialRegistrado(VTAModVentaTOportunidadDTO dto);
        CFGRespuestaGenericaDTO AsignarAsesor(VTAModVentaAsignarAsesorDTO dto);
        VTAModVentaImportarLinkedinResultadoDTO ImportarProcesadoLinkedin(DateTime? fechaInicio, DateTime? fechaFin);
    }
}