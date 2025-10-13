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
                        CodigoLanzamiento = o.CodigoLanzamiento ?? string.Empty,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,

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
                                FechaCreacion = h.FechaCreacion,

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
                                    Estado = h.Asesor.Estado
                                },

                                EstadoReferencia = h.EstadoReferencia == null ? null : new VTAModVentaTEstadoDTO
                                {
                                    Id = h.EstadoReferencia.Id,
                                    Nombre = h.EstadoReferencia.Nombre,
                                    Descripcion = h.EstadoReferencia.Descripcion,
                                    IdMigracion = h.EstadoReferencia.IdMigracion,
                                    Estado = h.EstadoReferencia.EstadoControl
                                },

                                Motivo = h.Motivo == null ? null : new VTAModVentaTMotivoDTO
                                {
                                    Id = h.Motivo.Id,
                                    Detalle = h.Motivo.Detalle,
                                    IdMigracion = h.Motivo.IdMigracion,
                                    Estado = h.Motivo.Estado
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
                                IdMigracion = hi.IdMigracion,
                                Estado = hi.Estado
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
                        CodigoLanzamiento = o.CodigoLanzamiento ?? string.Empty,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,

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
                                FechaCreacion = h.FechaCreacion,

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
                                    Estado = h.Asesor.Estado
                                },

                                EstadoReferencia = h.EstadoReferencia == null ? null : new VTAModVentaTEstadoDTO
                                {
                                    Id = h.EstadoReferencia.Id,
                                    Nombre = h.EstadoReferencia.Nombre,
                                    Descripcion = h.EstadoReferencia.Descripcion,
                                    IdMigracion = h.EstadoReferencia.IdMigracion,
                                    Estado = h.EstadoReferencia.EstadoControl
                                },

                                Motivo = h.Motivo == null ? null : new VTAModVentaTMotivoDTO
                                {
                                    Id = h.Motivo.Id,
                                    Detalle = h.Motivo.Detalle,
                                    IdMigracion = h.Motivo.IdMigracion,
                                    Estado = h.Motivo.Estado
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
                                IdMigracion = h.IdMigracion,
                                Estado = h.Estado
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
                                IdMigracion = reader.IsDBNull(reader.GetOrdinal("Motivo_IdMigracion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Motivo_IdMigracion")),
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
                            IdMigracion = reader.IsDBNull(reader.GetOrdinal("IdMigracion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdMigracion")),
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
                            IdMigracion = reader.IsDBNull(reader.GetOrdinal("IdMigracion")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdMigracion")),
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
