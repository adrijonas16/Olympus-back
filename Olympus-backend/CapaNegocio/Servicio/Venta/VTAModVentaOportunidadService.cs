using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Linq;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaOportunidadService : IVTAModVentaOportunidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaOportunidadService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTOportunidadDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.OportunidadRepository.ObtenerTodos()
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Estado = o.Estado
                    })
                    .ToList();

                respuesta.Oportunidad = lista;
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

        public VTAModVentaTOportunidadDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTOportunidadDTO();
            try
            {
                var ent = _unitOfWork.OportunidadRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdPersona = ent.IdPersona;
                    dto.CodigoLanzamiento = ent.CodigoLanzamiento ?? string.Empty;
                    dto.Estado = ent.Estado;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new Oportunidad
                {
                    IdPersona = dto.IdPersona,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.OportunidadRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.OportunidadRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdPersona = dto.IdPersona;
                ent.CodigoLanzamiento = dto.CodigoLanzamiento;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.OportunidadRepository.Actualizar(ent);
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
                var success = _unitOfWork.OportunidadRepository.Eliminar(id);
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

        public VTAModVentaTOportunidadDTORPT ObtenerPorPersona(int idPersona)
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.OportunidadRepository.ObtenerTodos()
                    .Where(o => o.IdPersona == idPersona)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Estado = o.Estado
                    })
                    .ToList();

                respuesta.Oportunidad = lista;
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

        public VTAModVentaTControlOportunidadDTORPT ObtenerControlOportunidadesPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTControlOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.ControlOportunidadRepository.ObtenerTodos()
                    .Where(c => c.IdOportunidad == idOportunidad)
                    .Select(c => new VTAModVentaTControlOportunidadDTO
                    {
                        Id = c.Id,
                        IdOportunidad = c.IdOportunidad,
                        Nombre = c.Nombre,
                        Url = c.Url ?? string.Empty,
                        Detalle = c.Detalle ?? string.Empty,
                        IdMigracion = c.IdMigracion,
                        Estado = c.Estado
                    })
                    .ToList();

                respuesta.ControlOportunidad = lista;
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

        public VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();
            try
            {
                var lista = _unitOfWork.HistorialInteraccionRepository.ObtenerTodos()
                    .Where(h => h.IdOportunidad == idOportunidad)
                    .Select(h => new VTAModVentaTHistorialInteraccionDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        Detalle = h.Detalle ?? string.Empty,
                        Tipo = h.Tipo ?? string.Empty,
                        Celular = h.Celular ?? string.Empty,
                        FechaRecordatorio = h.FechaRecordatorio,
                        IdMigracion = h.IdMigracion,
                        Estado = h.Estado
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

        public VTAModVentaTHistorialEstadoDTORPT ObtenerHistorialEstadoPorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaTHistorialEstadoDTORPT();
            try
            {
                var lista = _unitOfWork.HistorialEstadoRepository.ObtenerTodos()
                    .Where(h => h.IdOportunidad == idOportunidad)
                    .Select(h => new VTAModVentaTHistorialEstadoDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        IdAsesor = h.IdAsesor,
                        IdMotivo = h.IdMotivo,
                        IdEstado = h.IdEstado,
                        Observaciones = h.Observaciones ?? string.Empty,
                        CantidadLlamadasContestadas = h.CantidadLlamadasContestadas,
                        CantidadLlamadasNoContestadas = h.CantidadLlamadasNoContestadas,
                        IdMigracion = h.IdMigracion,
                        Estado = h.Estado
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
    }
}
