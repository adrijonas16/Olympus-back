using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaHistorialInteraccionService : IVTAModVentaHistorialInteraccionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHistorialInteraccionService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTHistorialInteraccionDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();
            try
            {
                var lista = _unitOfWork.HistorialInteraccionRepository.ObtenerTodos()
                    .Select(h => new VTAModVentaTHistorialInteraccionDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        Detalle = h.Detalle,
                        Tipo = h.Tipo,
                        Celular = h.Celular,
                        FechaRecordatorio = h.FechaRecordatorio,
                        Estado = h.Estado,
                        IdMigracion = h.IdMigracion
                    })
                    .ToList();

                respuesta.HistorialInteraccion = lista;
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        public VTAModVentaTHistorialInteraccionDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTHistorialInteraccionDTO();
            try
            {
                var ent = _unitOfWork.HistorialInteraccionRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdOportunidad = ent.IdOportunidad;
                    dto.Detalle = ent.Detalle;
                    dto.Tipo = ent.Tipo;
                    dto.Celular = ent.Celular;
                    dto.FechaRecordatorio = ent.FechaRecordatorio;
                    dto.Estado = ent.Estado;
                    dto.IdMigracion = ent.IdMigracion;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTHistorialInteraccionDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new HistorialInteraccion
                {
                    IdOportunidad = dto.IdOportunidad,
                    Detalle = dto.Detalle,
                    Tipo = dto.Tipo,
                    Celular = dto.Celular,
                    FechaRecordatorio = dto.FechaRecordatorio,
                    Estado = dto.Estado,
                    IdMigracion = dto.IdMigracion,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.HistorialInteraccionRepository.Insertar(ent);
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTHistorialInteraccionDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.HistorialInteraccionRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdOportunidad = dto.IdOportunidad;
                ent.Detalle = dto.Detalle;
                ent.Tipo = dto.Tipo;
                ent.Celular = dto.Celular;
                ent.FechaRecordatorio = dto.FechaRecordatorio;
                ent.Estado = dto.Estado;
                ent.IdMigracion = dto.IdMigracion;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.HistorialInteraccionRepository.Actualizar(ent);
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        public CFGRespuestaGenericaDTO Eliminar(int id)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var success = _unitOfWork.HistorialInteraccionRepository.Eliminar(id);
                if (!success)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }
    }
}
