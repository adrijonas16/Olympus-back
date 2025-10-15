using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaHistorialEstadoService : IVTAModVentaHistorialEstadoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHistorialEstadoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTHistorialEstadoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTHistorialEstadoDTORPT();
            try
            {
                var lista = _unitOfWork.HistorialEstadoRepository.ObtenerTodos()
                    .Select(h => new VTAModVentaTHistorialEstadoDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        IdAsesor = h.IdAsesor,
                        IdMotivo = h.IdMotivo,
                        IdEstado = h.IdEstado,
                        Observaciones = h.Observaciones,
                        CantidadLlamadasContestadas = h.CantidadLlamadasContestadas ?? 0,
                        CantidadLlamadasNoContestadas = h.CantidadLlamadasNoContestadas ?? 0,
                        Estado = h.Estado,
                        UsuarioCreacion = h.UsuarioCreacion,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioModificacion = h.UsuarioModificacion,
                        FechaModificacion = h.FechaModificacion,
                    })
                    .ToList();

                respuesta.HistorialEstado = lista;
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

        public VTAModVentaTHistorialEstadoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTHistorialEstadoDTO();
            try
            {
                var ent = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdOportunidad = ent.IdOportunidad;
                    dto.IdAsesor = ent.IdAsesor;
                    dto.IdMotivo = ent.IdMotivo;
                    dto.IdEstado = ent.IdEstado;
                    dto.Observaciones = ent.Observaciones;
                    dto.CantidadLlamadasContestadas = ent.CantidadLlamadasContestadas ?? 0;
                    dto.CantidadLlamadasNoContestadas = ent.CantidadLlamadasNoContestadas ?? 0;
                    dto.Estado = ent.Estado;
                    dto.UsuarioCreacion = ent.UsuarioCreacion;
                    dto.FechaCreacion = ent.FechaCreacion;
                    dto.UsuarioModificacion = ent.UsuarioModificacion;
                    dto.FechaModificacion = ent.FechaModificacion;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTHistorialEstadoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new HistorialEstado
                {
                    IdOportunidad = dto.IdOportunidad,
                    IdAsesor = dto.IdAsesor,
                    IdMotivo = dto.IdMotivo,
                    IdEstado = dto.IdEstado,
                    Observaciones = dto.Observaciones,
                    CantidadLlamadasContestadas = dto.CantidadLlamadasContestadas,
                    CantidadLlamadasNoContestadas = dto.CantidadLlamadasNoContestadas,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.HistorialEstadoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTHistorialEstadoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdOportunidad = dto.IdOportunidad;
                ent.IdAsesor = dto.IdAsesor;
                ent.IdMotivo = dto.IdMotivo;
                ent.IdEstado = dto.IdEstado;
                ent.Observaciones = dto.Observaciones;
                ent.CantidadLlamadasContestadas = dto.CantidadLlamadasContestadas;
                ent.CantidadLlamadasNoContestadas = dto.CantidadLlamadasNoContestadas;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.HistorialEstadoRepository.Actualizar(ent);
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
                var success = _unitOfWork.HistorialEstadoRepository.Eliminar(id);
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
