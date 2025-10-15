using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Data;
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
                        IdLanzamiento = o.IdLanzamiento,
                        CodigoLanzamiento = o.Lanzamiento != null ? o.Lanzamiento.CodigoLanzamiento : string.Empty,
                        Estado = o.Estado,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioModificacion = o.UsuarioModificacion,
                        FechaModificacion = o.FechaModificacion
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
                    dto.IdLanzamiento = ent.IdLanzamiento;
                    dto.CodigoLanzamiento = ent.Lanzamiento != null ? ent.Lanzamiento.CodigoLanzamiento : string.Empty;
                    dto.Estado = ent.Estado;
                    dto.UsuarioCreacion = ent.UsuarioCreacion ?? string.Empty;
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                // Validar Lanzamiento obligatorio
                var lanzamiento = _unitOfWork.LanzamientoRepository.ObtenerPorId(dto.IdLanzamiento);
                if (lanzamiento == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Lanzamiento no encontrado.";
                    return respuesta;
                }

                // Validar Asesor si fue enviado
                if (dto.IdAsesor.HasValue)
                {
                    var asesor = _unitOfWork.AsesorRepository.ObtenerPorId(dto.IdAsesor.Value);
                    if (asesor == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Asesor no encontrado.";
                        return respuesta;
                    }
                }

                // Validar que exista el estado inicial (Id = 2)
                var estadoInicial = _unitOfWork.EstadoRepository.ObtenerPorId(2);
                if (estadoInicial == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Estado inicial no encontrado";
                    return respuesta;
                }

                _unitOfWork.BeginTransactionAsync().GetAwaiter().GetResult();

                // Crear Oportunidad
                var oportunidad = new Oportunidad
                {
                    IdPersona = dto.IdPersona,
                    IdLanzamiento = dto.IdLanzamiento,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.OportunidadRepository.Insertar(oportunidad);

                // Crear HistorialEstado vinculado a la Oportunidad
                var historialEstado = new HistorialEstado
                {
                    Oportunidad = oportunidad,
                    IdAsesor = dto.IdAsesor,
                    IdMotivo = null,
                    IdEstado = estadoInicial.Id,
                    Observaciones = "Estado inicial al crear oportunidad",
                    CantidadLlamadasContestadas = 0,
                    CantidadLlamadasNoContestadas = 0,
                    Estado = true,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.HistorialEstadoRepository.Insertar(historialEstado);

                // Crear HistorialInteraccion tipo "Recordatorio"
                var historialInteraccion = new HistorialInteraccion
                {
                    Oportunidad = oportunidad,
                    Detalle = "Recordatorio generado al crear oportunidad",
                    Tipo = "Recordatorio",
                    Celular = string.Empty,
                    FechaRecordatorio = dto.FechaRecordatorio,
                    Estado = true,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.HistorialInteraccionRepository.Insertar(historialInteraccion);

                // Guardar todo en una sola operación y confirmar transacción
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                _unitOfWork.CommitTransactionAsync().GetAwaiter().GetResult();

                var dtoCreado = new VTAModVentaTOportunidadDTO
                {
                    Id = oportunidad.Id,
                    IdPersona = oportunidad.IdPersona,
                    IdLanzamiento = oportunidad.IdLanzamiento,
                    CodigoLanzamiento = lanzamiento?.CodigoLanzamiento ?? string.Empty,
                    Estado = oportunidad.Estado,
                    IdAsesor = dto.IdAsesor,
                    FechaRecordatorio = dto.FechaRecordatorio,
                    FechaCreacion = oportunidad.FechaCreacion,
                    UsuarioCreacion = oportunidad.UsuarioCreacion,
                    FechaModificacion = oportunidad.FechaModificacion,
                    UsuarioModificacion = oportunidad.UsuarioModificacion
                };

                respuesta.Oportunidad.Add(dtoCreado);
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
            }
            catch (Exception ex)
            {
                try { 
                    _unitOfWork.RollbackTransactionAsync().GetAwaiter().GetResult(); 
                } 
                catch { }
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                var lanzamiento = _unitOfWork.LanzamientoRepository.ObtenerPorId(dto.IdLanzamiento);
                if (lanzamiento == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                var ent = _unitOfWork.OportunidadRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdPersona = dto.IdPersona;
                ent.IdLanzamiento = dto.IdLanzamiento;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.OportunidadRepository.Actualizar(ent);
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                var dtoActualizado = new VTAModVentaTOportunidadDTO
                {
                    Id = ent.Id,
                    IdPersona = ent.IdPersona,
                    IdLanzamiento = ent.IdLanzamiento,
                    CodigoLanzamiento = lanzamiento?.CodigoLanzamiento ?? string.Empty,
                    Estado = ent.Estado,
                    IdAsesor = dto.IdAsesor,
                    FechaRecordatorio = dto.FechaRecordatorio,
                    FechaCreacion = ent.FechaCreacion,
                    UsuarioCreacion = ent.UsuarioCreacion,
                    FechaModificacion = ent.FechaModificacion,
                    UsuarioModificacion = ent.UsuarioModificacion
                };

                respuesta.Oportunidad.Add(dtoActualizado);
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
                        IdLanzamiento = o.IdLanzamiento,
                        CodigoLanzamiento = o.Lanzamiento != null ? o.Lanzamiento.CodigoLanzamiento : string.Empty,
                        Estado = o.Estado,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioModificacion = o.UsuarioModificacion,
                        FechaModificacion = o.FechaModificacion
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
                        Estado = c.Estado,
                        UsuarioCreacion = c.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioModificacion = c.UsuarioModificacion,
                        FechaModificacion = c.FechaModificacion
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
                        Estado = h.Estado,
                        UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioModificacion = h.UsuarioModificacion,
                        FechaModificacion = h.FechaModificacion
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
                        Estado = h.Estado,
                        UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioModificacion = h.UsuarioModificacion ?? string.Empty,
                        FechaModificacion = h.FechaModificacion 
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

        public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle()
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
            try
            {
                var lista = _unitOfWork.OportunidadRepository.ObtenerTodos()
                    .AsNoTracking()
                    .Select(o => new VTAModVentaTOportunidadDetalleDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        PersonaNombres = o.Persona != null ? o.Persona.Nombres : string.Empty,
                        PersonaApellidos = o.Persona != null ? o.Persona.Apellidos : string.Empty,
                        IdLanzamiento = o.IdLanzamiento,
                        CodigoLanzamiento = o.Lanzamiento != null ? o.Lanzamiento.CodigoLanzamiento : string.Empty,
                        Estado = o.Estado,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioModificacion = o.UsuarioModificacion,
                        FechaModificacion = o.FechaModificacion,

                        HistorialEstado = o.HistorialEstado
                            .OrderByDescending(h => h.FechaCreacion)
                            .Select(h => new VTAModVentaTHistorialEstadoDetalleDTO
                            {
                                Id = h.Id,
                                IdOportunidad = h.IdOportunidad,
                                IdAsesor = h.IdAsesor,
                                IdMotivo = h.IdMotivo,
                                IdEstado = h.IdEstado,
                                Observaciones = h.Observaciones ?? string.Empty,
                                CantidadLlamadasContestadas = h.CantidadLlamadasContestadas,
                                CantidadLlamadasNoContestadas = h.CantidadLlamadasNoContestadas,
                                TotalMarcaciones = (h.CantidadLlamadasContestadas ?? 0) + (h.CantidadLlamadasNoContestadas ?? 0),
                                UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                                FechaCreacion = h.FechaCreacion,
                                UsuarioModificacion = h.UsuarioModificacion,
                                FechaModificacion = h.FechaModificacion,

                                Asesor = h.Asesor == null ? null : new VTAModVentaTAsesorDTO
                                {
                                    Id = h.Asesor.Id,
                                    IdPais = h.Asesor.IdPais,
                                    Nombres = h.Asesor.Nombres,
                                    Apellidos = h.Asesor.Apellidos,
                                    Celular = h.Asesor.Celular,
                                    PrefijoPaisCelular = h.Asesor.PrefijoPaisCelular,
                                    Correo = h.Asesor.Correo,
                                    AreaTrabajo = h.Asesor.AreaTrabajo,
                                    Cesado = h.Asesor.Cesado,
                                    Estado = h.Asesor.Estado,
                                    UsuarioCreacion = h.Asesor.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.Asesor.FechaCreacion,
                                    UsuarioModificacion = h.Asesor.UsuarioModificacion,
                                    FechaModificacion = h.Asesor.FechaModificacion
                                },

                                EstadoReferencia = h.EstadoReferencia == null ? null : new VTAModVentaTEstadoDTO
                                {
                                    Id = h.EstadoReferencia.Id,
                                    Nombre = h.EstadoReferencia.Nombre,
                                    Descripcion = h.EstadoReferencia.Descripcion,
                                    IdMigracion = h.EstadoReferencia.IdMigracion,
                                    Estado = h.EstadoReferencia.EstadoControl,
                                    UsuarioCreacion = h.EstadoReferencia.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.EstadoReferencia.FechaCreacion,
                                    UsuarioModificacion = h.EstadoReferencia.UsuarioModificacion,
                                    FechaModificacion = h.EstadoReferencia.FechaModificacion
                                },

                                Motivo = h.Motivo == null ? null : new VTAModVentaTMotivoDTO
                                {
                                    Id = h.Motivo.Id,
                                    Detalle = h.Motivo.Detalle,
                                    Estado = h.Motivo.Estado,
                                    UsuarioCreacion = h.Motivo.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.Motivo.FechaCreacion,
                                    UsuarioModificacion = h.Motivo.UsuarioModificacion,
                                    FechaModificacion = h.Motivo.FechaModificacion
                                }
                            })
                            .FirstOrDefault(),

                        HistorialInteraccion = o.HistorialInteracciones
                            .Where(hi => hi.Tipo == "Recordatorio")
                            .OrderByDescending(hi => hi.FechaRecordatorio) // ordenar por fecha de recordatorio descendente
                            .Select(hi => new VTAModVentaTHistorialInteraccionDTO
                            {
                                Id = hi.Id,
                                IdOportunidad = hi.IdOportunidad,
                                Detalle = hi.Detalle ?? string.Empty,
                                Tipo = hi.Tipo ?? string.Empty,
                                Celular = hi.Celular ?? string.Empty,
                                FechaRecordatorio = hi.FechaRecordatorio,
                                Estado = hi.Estado,
                                UsuarioCreacion = hi.UsuarioCreacion ?? string.Empty,
                                FechaCreacion = hi.FechaCreacion,
                                UsuarioModificacion = hi.UsuarioModificacion,
                                FechaModificacion = hi.FechaModificacion
                            })
                            .ToList()
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

        public VTAModVentaTOportunidadDetalleDTO ObtenerDetallePorId(int id)
        {
            var dto = new VTAModVentaTOportunidadDetalleDTO();
            try
            {
                var entidad = _unitOfWork.OportunidadRepository.ObtenerTodos()
                    .AsNoTracking()
                    .Where(o => o.Id == id)
                    .Select(o => new VTAModVentaTOportunidadDetalleDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        PersonaNombres = o.Persona != null ? o.Persona.Nombres : string.Empty,
                        PersonaApellidos = o.Persona != null ? o.Persona.Apellidos : string.Empty,
                        IdLanzamiento = o.IdLanzamiento,
                        CodigoLanzamiento = o.Lanzamiento != null ? o.Lanzamiento.CodigoLanzamiento : string.Empty,
                        Estado = o.Estado,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioModificacion = o.UsuarioModificacion,
                        FechaModificacion = o.FechaModificacion,

                        HistorialEstado = o.HistorialEstado
                            .OrderByDescending(h => h.FechaCreacion)
                            .Select(h => new VTAModVentaTHistorialEstadoDetalleDTO
                            {
                                Id = h.Id,
                                IdOportunidad = h.IdOportunidad,
                                IdAsesor = h.IdAsesor,
                                IdMotivo = h.IdMotivo,
                                IdEstado = h.IdEstado,
                                Observaciones = h.Observaciones ?? string.Empty,
                                CantidadLlamadasContestadas = h.CantidadLlamadasContestadas,
                                CantidadLlamadasNoContestadas = h.CantidadLlamadasNoContestadas,
                                TotalMarcaciones = (h.CantidadLlamadasContestadas ?? 0) + (h.CantidadLlamadasNoContestadas ?? 0),
                                UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                                FechaCreacion = h.FechaCreacion,
                                UsuarioModificacion = h.UsuarioModificacion,
                                FechaModificacion = h.FechaModificacion,

                                Asesor = h.Asesor == null ? null : new VTAModVentaTAsesorDTO
                                {
                                    Id = h.Asesor.Id,
                                    IdPais = h.Asesor.IdPais,
                                    Nombres = h.Asesor.Nombres,
                                    Apellidos = h.Asesor.Apellidos,
                                    Celular = h.Asesor.Celular,
                                    PrefijoPaisCelular = h.Asesor.PrefijoPaisCelular,
                                    Correo = h.Asesor.Correo,
                                    AreaTrabajo = h.Asesor.AreaTrabajo,
                                    Cesado = h.Asesor.Cesado,
                                    Estado = h.Asesor.Estado,
                                    UsuarioCreacion = h.Asesor.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.Asesor.FechaCreacion,
                                    UsuarioModificacion = h.Asesor.UsuarioModificacion,
                                    FechaModificacion = h.Asesor.FechaModificacion
                                },

                                EstadoReferencia = h.EstadoReferencia == null ? null : new VTAModVentaTEstadoDTO
                                {
                                    Id = h.EstadoReferencia.Id,
                                    Nombre = h.EstadoReferencia.Nombre,
                                    Descripcion = h.EstadoReferencia.Descripcion,
                                    IdMigracion = h.EstadoReferencia.IdMigracion,
                                    Estado = h.EstadoReferencia.EstadoControl,
                                    UsuarioCreacion = h.EstadoReferencia.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.EstadoReferencia.FechaCreacion,
                                    UsuarioModificacion = h.EstadoReferencia.UsuarioModificacion,
                                    FechaModificacion = h.EstadoReferencia.FechaModificacion
                                },

                                Motivo = h.Motivo == null ? null : new VTAModVentaTMotivoDTO
                                {
                                    Id = h.Motivo.Id,
                                    Detalle = h.Motivo.Detalle,
                                    Estado = h.Motivo.Estado,
                                    UsuarioCreacion = h.Motivo.UsuarioCreacion ?? string.Empty,
                                    FechaCreacion = h.Motivo.FechaCreacion,
                                    UsuarioModificacion = h.Motivo.UsuarioModificacion,
                                    FechaModificacion = h.Motivo.FechaModificacion
                                }
                            })
                            .FirstOrDefault(),

                        HistorialInteraccion = o.HistorialInteracciones
                            .OrderBy(h => h.FechaModificacion)
                            .Select(h => new VTAModVentaTHistorialInteraccionDTO
                            {
                                Id = h.Id,
                                IdOportunidad = h.IdOportunidad,
                                Detalle = h.Detalle ?? string.Empty,
                                Tipo = h.Tipo ?? string.Empty,
                                Celular = h.Celular ?? string.Empty,
                                FechaModificacion = h.FechaModificacion,
                                Estado = h.Estado,
                                UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                                FechaCreacion = h.FechaCreacion,
                                UsuarioModificacion = h.UsuarioModificacion,
                                FechaRecordatorio = h.FechaRecordatorio
                            })
                            .ToList()
                    })
                    .FirstOrDefault();

                if (entidad != null) dto = entidad;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }

            return dto;
        }

        public VTAModVentaTOportunidadDetalleDTORPT ObtenerTodasConDetalle_SP_Multi(string? tipoInteraccion = null)
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
            try
            {
                var listaOportunidades = new List<VTAModVentaTOportunidadDetalleDTO>();
                var dictHistorial = new Dictionary<int, VTAModVentaTHistorialEstadoDetalleDTO?>();
                var dictControles = new Dictionary<int, List<VTAModVentaTControlOportunidadDTO>>();
                var dictInteracciones = new Dictionary<int, List<VTAModVentaTHistorialInteraccionDTO>>();

                var connString = _config.GetConnectionString("DefaultConnection");
                using var conn = new SqlConnection(connString);
                using var cmd = new SqlCommand("[adm].[sp_GetOportunidadesConDetalle_Multi]", conn)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 120
                };

                cmd.Parameters.Add(new SqlParameter("@TipoInteraccion", SqlDbType.VarChar, 100)
                {
                    Value = tipoInteraccion as object ?? DBNull.Value
                });

                conn.Open();
                // Al cerrar reader también se cierre la conexión
                using var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                // OPORTUNIDADES
                while (reader.Read())
                {
                    var o = new VTAModVentaTOportunidadDetalleDTO
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        IdPersona = reader.GetInt32(reader.GetOrdinal("IdPersona")),
                        PersonaNombres = reader.IsDBNull(reader.GetOrdinal("PersonaNombres")) ? string.Empty : reader.GetString(reader.GetOrdinal("PersonaNombres")),
                        PersonaApellidos = reader.IsDBNull(reader.GetOrdinal("PersonaApellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("PersonaApellidos")),
                        CodigoLanzamiento = reader.IsDBNull(reader.GetOrdinal("CodigoLanzamiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoLanzamiento")),
                        Estado = !reader.IsDBNull(reader.GetOrdinal("Estado")) && reader.GetBoolean(reader.GetOrdinal("Estado")),
                        FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                        UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                        HistorialInteraccion = new List<VTAModVentaTHistorialInteraccionDTO>(),
                        HistorialEstado = null
                    };
                    listaOportunidades.Add(o);
                }

                // ÚLTIMO HISTORIALESTADO POR OPORTUNIDAD
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var idOportunidad = reader.IsDBNull(reader.GetOrdinal("IdOportunidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdOportunidad"));
                        var hist = new VTAModVentaTHistorialEstadoDetalleDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            IdOportunidad = idOportunidad,
                            IdAsesor = reader.IsDBNull(reader.GetOrdinal("IdAsesor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdAsesor")),
                            IdMotivo = reader.IsDBNull(reader.GetOrdinal("IdMotivo")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdMotivo")),
                            IdEstado = reader.IsDBNull(reader.GetOrdinal("IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdEstado")),
                            Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observaciones")),
                            CantidadLlamadasContestadas = reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasContestadas")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasContestadas")),
                            CantidadLlamadasNoContestadas = reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasNoContestadas")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasNoContestadas")),
                            TotalMarcaciones = (reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasContestadas")))
                                            + (reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasNoContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasNoContestadas"))),
                            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                            // Asesor
                            Asesor = reader.IsDBNull(reader.GetOrdinal("Asesor_Id")) ? null : new VTAModVentaTAsesorDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Asesor_Id")),
                                IdPais = reader.IsDBNull(reader.GetOrdinal("Asesor_IdPais")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Asesor_IdPais")),
                                Nombres = reader.IsDBNull(reader.GetOrdinal("Asesor_Nombres")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Nombres")),
                                Apellidos = reader.IsDBNull(reader.GetOrdinal("Asesor_Apellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Apellidos")),
                                Celular = reader.IsDBNull(reader.GetOrdinal("Asesor_Celular")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Celular")),
                                PrefijoPaisCelular = reader.IsDBNull(reader.GetOrdinal("Asesor_PrefijoPaisCelular")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_PrefijoPaisCelular")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Asesor_Correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Correo")),
                                AreaTrabajo = reader.IsDBNull(reader.GetOrdinal("Asesor_AreaTrabajo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_AreaTrabajo")),
                                Cesado = !reader.IsDBNull(reader.GetOrdinal("Asesor_Cesado")) && reader.GetBoolean(reader.GetOrdinal("Asesor_Cesado")),
                                Estado = !reader.IsDBNull(reader.GetOrdinal("Asesor_Estado")) && reader.GetBoolean(reader.GetOrdinal("Asesor_Estado"))
                            },
                            EstadoReferencia = reader.IsDBNull(reader.GetOrdinal("Estado_Id")) ? null : new VTAModVentaTEstadoDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Estado_Id")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Estado_Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado_Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Estado_Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Estado_Descripcion")),
                                IdMigracion = reader.IsDBNull(reader.GetOrdinal("Estado_IdMigracion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Estado_IdMigracion")),
                                Estado = !reader.IsDBNull(reader.GetOrdinal("Estado_Estado")) && reader.GetBoolean(reader.GetOrdinal("Estado_Estado"))
                            },
                            Motivo = reader.IsDBNull(reader.GetOrdinal("Motivo_Id")) ? null : new VTAModVentaTMotivoDTO
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Motivo_Id")),
                                Detalle = reader.IsDBNull(reader.GetOrdinal("Motivo_Detalle")) ? string.Empty : reader.GetString(reader.GetOrdinal("Motivo_Detalle")),
                                Estado = !reader.IsDBNull(reader.GetOrdinal("Motivo_Estado")) && reader.GetBoolean(reader.GetOrdinal("Motivo_Estado"))
                            }
                        };

                        dictHistorial[idOportunidad] = hist;
                    }
                }

                // CONTROL OPORTUNIDAD
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var idOpp = reader.GetInt32(reader.GetOrdinal("IdOportunidad"));
                        var ctrl = new VTAModVentaTControlOportunidadDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            IdOportunidad = idOpp,
                            Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                            Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? string.Empty : reader.GetString(reader.GetOrdinal("Url")),
                            Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? string.Empty : reader.GetString(reader.GetOrdinal("Detalle")),
                            Estado = !reader.IsDBNull(reader.GetOrdinal("Estado")) && reader.GetBoolean(reader.GetOrdinal("Estado"))
                        };
                        if (!dictControles.TryGetValue(idOpp, out var list)) { list = new List<VTAModVentaTControlOportunidadDTO>(); dictControles[idOpp] = list; }
                        list.Add(ctrl);
                    }
                }

                // HISTORIAL INTERACCION
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var idOpp = reader.GetInt32(reader.GetOrdinal("IdOportunidad"));
                        var hi = new VTAModVentaTHistorialInteraccionDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            IdOportunidad = idOpp,
                            Detalle = reader.IsDBNull(reader.GetOrdinal("Detalle")) ? string.Empty : reader.GetString(reader.GetOrdinal("Detalle")),
                            Tipo = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                            Celular = reader.IsDBNull(reader.GetOrdinal("Celular")) ? string.Empty : reader.GetString(reader.GetOrdinal("Celular")),
                            FechaRecordatorio = reader.IsDBNull(reader.GetOrdinal("FechaRecordatorio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRecordatorio")),
                            Estado = !reader.IsDBNull(reader.GetOrdinal("Estado")) && reader.GetBoolean(reader.GetOrdinal("Estado"))
                        };
                        if (!dictInteracciones.TryGetValue(idOpp, out var list)) { list = new List<VTAModVentaTHistorialInteraccionDTO>(); dictInteracciones[idOpp] = list; }
                        list.Add(hi);
                    }
                }

                // Mapear
                foreach (var o in listaOportunidades)
                {
                    dictHistorial.TryGetValue(o.Id, out var hist); o.HistorialEstado = hist;
                    dictControles.TryGetValue(o.Id, out var ctrls); //
                    o.HistorialInteraccion = dictInteracciones.TryGetValue(o.Id, out var inters) ? inters : new List<VTAModVentaTHistorialInteraccionDTO>();
                }

                respuesta.Oportunidad = listaOportunidades;
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
                // conexión se cierra automáticamente al salir del using(reader) por CommandBehavior.CloseConnection
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message + " | " + ex.StackTrace;
            }
            return respuesta;
        }
    }
}
