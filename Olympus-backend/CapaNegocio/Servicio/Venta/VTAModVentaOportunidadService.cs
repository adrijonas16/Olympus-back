using CapaDatos.DataContext;
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
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaOportunidadService : IVTAModVentaOportunidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;
        private readonly OlympusContext _context;

        public VTAModVentaOportunidadService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService, OlympusContext context)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
            _context = context;
        }

        public VTAModVentaTOportunidadDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Include(o => o.PotencialCliente)
                        .ThenInclude(pc => pc.Persona)
                    .Include(o => o.Producto)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPotencialCliente = o.IdPotencialCliente,
                        PersonaNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null 
                            ? ((o.PotencialCliente.Persona.Nombres ?? string.Empty) + " " + (o.PotencialCliente.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Origen = o.Origen,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion
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
                var ent = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Include(o => o.PotencialCliente)
                        .ThenInclude(pc => pc.Persona)
                    .Include(o => o.Producto)
                    .Where(o => o.Id == id)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPotencialCliente = o.IdPotencialCliente,
                        PersonaNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null
                            ? ((o.PotencialCliente.Persona.Nombres ?? string.Empty) + " " + (o.PotencialCliente.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Origen = o.Origen,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion
                    })
                    .FirstOrDefault();

                if (ent != null) dto = ent;
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
                // Validar Persona
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

                // Validar Producto si viene IdProducto
                if (dto.IdProducto.HasValue)
                {
                    var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (producto == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                var ent = new Oportunidad
                {
                    IdPotencialCliente = dto.IdPotencialCliente,
                    IdProducto = dto.IdProducto,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Origen = dto.Origen,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
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

                // Validar Persona
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

                // Validar Producto si viene IdProducto
                if (dto.IdProducto.HasValue)
                {
                    var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (producto == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                ent.IdPotencialCliente = dto.IdPotencialCliente;
                ent.IdProducto = dto.IdProducto;
                ent.CodigoLanzamiento = dto.CodigoLanzamiento;
                ent.Origen = dto.Origen;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

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

        public VTAModVentaOportunidadDetalleDTORPT ObtenerDetallePorId(int id)
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();

            try
            {
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SP_ObtenerDetalleOportunidadPorId";

                var p = cmd.CreateParameter();
                p.ParameterName = "@IdOportunidad";
                p.DbType = DbType.Int32;
                p.Value = id;
                cmd.Parameters.Add(p);

                if (conn.State != ConnectionState.Open) conn.Open();

                using var reader = cmd.ExecuteReader();

                var ord = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
                int Ord(DbDataReader r, string name)
                {
                    if (!ord.TryGetValue(name, out var i))
                    {
                        i = r.GetOrdinal(name);
                        ord[name] = i;
                    }
                    return i;
                }

                if (reader.Read())
                {
                    int opId = reader.IsDBNull(Ord(reader, "OportunidadId")) ? 0 : reader.GetInt32(Ord(reader, "OportunidadId"));
                    int? idPotencial = reader.IsDBNull(Ord(reader, "IdPotencialCliente")) ? (int?)null : reader.GetInt32(Ord(reader, "IdPotencialCliente"));

                    string personaNombres = reader.IsDBNull(Ord(reader, "Persona_Nombres")) ? string.Empty : reader.GetString(Ord(reader, "Persona_Nombres"));
                    string personaApellidos = reader.IsDBNull(Ord(reader, "Persona_Apellidos")) ? string.Empty : reader.GetString(Ord(reader, "Persona_Apellidos"));

                    int idProducto = reader.IsDBNull(Ord(reader, "IdProducto")) ? 0 : reader.GetInt32(Ord(reader, "IdProducto"));
                    string productoNombre = reader.IsDBNull(Ord(reader, "Producto_Nombre")) ? string.Empty : reader.GetString(Ord(reader, "Producto_Nombre"));
                    string codigoLanzamiento = reader.IsDBNull(Ord(reader, "CodigoLanzamiento")) ? string.Empty : reader.GetString(Ord(reader, "CodigoLanzamiento"));
                    string origen = reader.IsDBNull(Ord(reader, "Origen")) ? string.Empty : reader.GetString(Ord(reader, "Origen"));
                    bool estado = reader.IsDBNull(Ord(reader, "Estado")) ? true : reader.GetBoolean(Ord(reader, "Estado"));
                    DateTime fechaCreacion = reader.IsDBNull(Ord(reader, "Oportunidad_FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(Ord(reader, "Oportunidad_FechaCreacion"));
                    string usuarioCreacion = reader.IsDBNull(Ord(reader, "Oportunidad_UsuarioCreacion")) ? string.Empty : reader.GetString(Ord(reader, "Oportunidad_UsuarioCreacion"));

                    int totalHistSinAsesor = reader.IsDBNull(Ord(reader, "TotalHistorialesSinAsesor")) ? 0 : reader.GetInt32(Ord(reader, "TotalHistorialesSinAsesor"));

                    var oportunidadDto = new VTAModVentaOportunidadDetalleDTO
                    {
                        Id = opId,
                        IdPotencialCliente = idPotencial ?? 0,
                        PersonaNombre = $"{personaNombres} {personaApellidos}".Trim(),
                        PersonaCorreo = reader.IsDBNull(Ord(reader, "Persona_Correo")) ? string.Empty : reader.GetString(Ord(reader, "Persona_Correo")),
                        IdProducto = idProducto,
                        ProductoNombre = productoNombre ?? string.Empty,
                        CodigoLanzamiento = codigoLanzamiento ?? string.Empty,
                        Origen = origen,
                        Estado = estado,
                        TotalOportunidadesPersona = totalHistSinAsesor,
                        FechaCreacion = fechaCreacion,
                        UsuarioCreacion = usuarioCreacion ?? string.Empty
                    };

                    // Si hay UltimoHist_Id, construir historial DTO
                    if (!reader.IsDBNull(Ord(reader, "UltimoHist_Id")))
                    {
                        var histDto = new VTAModVentaTHistorialEstadoDetalleDTO
                        {
                            Id = reader.GetInt32(Ord(reader, "UltimoHist_Id")),
                            IdOportunidad = reader.IsDBNull(Ord(reader, "UltimoHist_IdOportunidad")) ? opId : reader.GetInt32(Ord(reader, "UltimoHist_IdOportunidad")),
                            IdAsesor = reader.IsDBNull(Ord(reader, "UltimoHist_IdAsesor")) ? (int?)null : reader.GetInt32(Ord(reader, "UltimoHist_IdAsesor")),
                            IdEstado = reader.IsDBNull(Ord(reader, "UltimoHist_IdEstado")) ? (int?)null : reader.GetInt32(Ord(reader, "UltimoHist_IdEstado")),
                            Observaciones = reader.IsDBNull(Ord(reader, "UltimoHist_Observaciones")) ? string.Empty : reader.GetString(Ord(reader, "UltimoHist_Observaciones")),
                            CantidadLlamadasContestadas = reader.IsDBNull(Ord(reader, "UltimoHist_CantidadLlamadasContestadas")) ? (int?)0 : reader.GetInt32(Ord(reader, "UltimoHist_CantidadLlamadasContestadas")),
                            CantidadLlamadasNoContestadas = reader.IsDBNull(Ord(reader, "UltimoHist_CantidadLlamadasNoContestadas")) ? (int?)0 : reader.GetInt32(Ord(reader, "UltimoHist_CantidadLlamadasNoContestadas")),
                            Estado = true,
                            FechaCreacion = reader.IsDBNull(Ord(reader, "UltimoHist_FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(Ord(reader, "UltimoHist_FechaCreacion")),
                            FechaModificacion = reader.IsDBNull(Ord(reader, "UltimoHist_FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(Ord(reader, "UltimoHist_FechaModificacion")),
                            UsuarioCreacion = reader.IsDBNull(Ord(reader, "UltimoHist_UsuarioCreacion")) ? string.Empty : reader.GetString(Ord(reader, "UltimoHist_UsuarioCreacion")),
                            UsuarioModificacion = reader.IsDBNull(Ord(reader, "UltimoHist_UsuarioModificacion")) ? string.Empty : reader.GetString(Ord(reader, "UltimoHist_UsuarioModificacion"))
                        };

                        // Asesor info
                        if (!reader.IsDBNull(Ord(reader, "Asesor_Id")))
                        {
                            var asesorDto = new VTAModVentaTAsesorDTO
                            {
                                Id = reader.GetInt32(Ord(reader, "Asesor_Id")),
                                Nombres = reader.IsDBNull(Ord(reader, "Asesor_Nombres")) ? string.Empty : reader.GetString(Ord(reader, "Asesor_Nombres")),
                                Apellidos = reader.IsDBNull(Ord(reader, "Asesor_Apellidos")) ? string.Empty : reader.GetString(Ord(reader, "Asesor_Apellidos")),
                                Correo = reader.IsDBNull(Ord(reader, "Asesor_Correo")) ? string.Empty : reader.GetString(Ord(reader, "Asesor_Correo")),
                                Celular = reader.IsDBNull(Ord(reader, "Asesor_Celular")) ? string.Empty : reader.GetString(Ord(reader, "Asesor_Celular"))
                            };
                            histDto.Asesor = asesorDto;
                        }

                        // Estado referencia y Tipo
                        if (!reader.IsDBNull(Ord(reader, "Estado_Id")))
                        {
                            var estadoDto = new VTAModVentaTEstadoDTO
                            {
                                Id = reader.GetInt32(Ord(reader, "Estado_Id")),
                                Nombre = reader.IsDBNull(Ord(reader, "Estado_Nombre")) ? string.Empty : reader.GetString(Ord(reader, "Estado_Nombre")),
                                IdTipo = reader.IsDBNull(Ord(reader, "Estado_IdTipo")) ? 0 : reader.GetInt32(Ord(reader, "Estado_IdTipo")),
                                TipoNombre = reader.IsDBNull(Ord(reader, "Tipo_Nombre")) ? string.Empty : reader.GetString(Ord(reader, "Tipo_Nombre")),
                                TipoCategoria = reader.IsDBNull(Ord(reader, "Tipo_Categoria")) ? string.Empty : reader.GetString(Ord(reader, "Tipo_Categoria"))
                            };
                            histDto.EstadoReferencia = estadoDto;
                        }

                        respuesta.HistorialActual.Add(histDto);
                    }

                    // Add oportunidad to response
                    respuesta.Oportunidad.Add(oportunidadDto);
                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.Empty;
                }
                else
                {
                    // No encontrado
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new VTAModVentaOportunidadDetalleDTORPT
                {
                    Codigo = SR._C_ERROR_CRITICO,
                    Mensaje = ex.Message
                };
            }
        }


        public VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int id, int? idTipo = null)
        {
            var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();

            try
            {
                var query = _unitOfWork.HistorialInteraccionRepository
                            .Query()
                            .AsNoTracking()
                            .Where(h => h.IdOportunidad == id);

                if (idTipo.HasValue)
                    query = query.Where(h => h.IdTipo == idTipo.Value);

                var lista = query
                    .OrderBy(h => h.FechaCreacion)
                    .Select(h => new VTAModVentaTHistorialInteraccionDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        IdTipo = h.IdTipo,
                        Detalle = h.Detalle ?? string.Empty,
                        Celular = h.Celular ?? string.Empty,
                        FechaRecordatorio = h.FechaRecordatorio,
                        Estado = h.Estado,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
                    })
                    .ToList();

                respuesta.HistorialInteracciones = lista;
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

        public VTAModVentaTOportunidadDetalleDTORPT ObtenerHistorialEstadoPorOportunidad(int IdOportunidad)
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();
            try
            {
                var lista = new List<VTAModVentaTHistorialEstadoDetalleDTO>();

                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SP_ObtenerHistorialEstadoPorOportunidad";
                cmd.Parameters.Add(new SqlParameter("@IdOportunidad", SqlDbType.Int) { Value = IdOportunidad });

                if (conn.State != ConnectionState.Open) conn.Open();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var h = new VTAModVentaTHistorialEstadoDetalleDTO
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("HistorialId")) ? 0 : reader.GetInt32(reader.GetOrdinal("HistorialId")),
                        IdOportunidad = reader.IsDBNull(reader.GetOrdinal("IdOportunidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdOportunidad")),
                        IdAsesor = reader.IsDBNull(reader.GetOrdinal("IdAsesor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdAsesor")),
                        IdEstado = reader.IsDBNull(reader.GetOrdinal("IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdEstado")),
                        IdOcurrencia = reader.IsDBNull(reader.GetOrdinal("IdOcurrencia")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdOcurrencia")),
                        Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observaciones")),
                        CantidadLlamadasContestadas = reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasContestadas")),
                        CantidadLlamadasNoContestadas = reader.IsDBNull(reader.GetOrdinal("CantidadLlamadasNoContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("CantidadLlamadasNoContestadas")),
                        Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? true : reader.GetBoolean(reader.GetOrdinal("Estado")),
                        FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                        UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                        FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                        UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion"))
                    };

                    // Asesor
                    if (!reader.IsDBNull(reader.GetOrdinal("Asesor_Id")))
                    {
                        h.Asesor = new VTAModVentaTAsesorDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Asesor_Id")),
                            Nombres = reader.IsDBNull(reader.GetOrdinal("Asesor_Nombres")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Nombres")),
                            Apellidos = reader.IsDBNull(reader.GetOrdinal("Asesor_Apellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Apellidos")),
                            Correo = reader.IsDBNull(reader.GetOrdinal("Asesor_Correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Correo")),
                            Celular = reader.IsDBNull(reader.GetOrdinal("Asesor_Celular")) ? string.Empty : reader.GetString(reader.GetOrdinal("Asesor_Celular"))
                        };
                    }

                    // EstadoReferencia
                    if (!reader.IsDBNull(reader.GetOrdinal("EstadoRef_Id")))
                    {
                        var tipoNombre = reader.IsDBNull(reader.GetOrdinal("Tipo_Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo_Nombre"));
                        var tipoCategoria = reader.IsDBNull(reader.GetOrdinal("Tipo_Categoria")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo_Categoria"));

                        h.EstadoReferencia = new VTAModVentaTEstadoDTO
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("EstadoRef_Id")),
                            Nombre = reader.IsDBNull(reader.GetOrdinal("EstadoRef_Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRef_Nombre")),
                            Descripcion = reader.IsDBNull(reader.GetOrdinal("EstadoRef_Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRef_Descripcion")),
                            IdTipo = reader.IsDBNull(reader.GetOrdinal("EstadoRef_IdTipo")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoRef_IdTipo")),
                            TipoNombre = tipoNombre,
                            TipoCategoria = tipoCategoria,
                            Estado = reader.IsDBNull(reader.GetOrdinal("EstadoRef_EstadoControl")) ? true : reader.GetBoolean(reader.GetOrdinal("EstadoRef_EstadoControl")),
                            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("EstadoRef_FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EstadoRef_FechaCreacion")),
                            UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("EstadoRef_UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRef_UsuarioCreacion")),
                            FechaModificacion = reader.IsDBNull(reader.GetOrdinal("EstadoRef_FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("EstadoRef_FechaModificacion")),
                            UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("EstadoRef_UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRef_UsuarioModificacion"))
                        };
                    }

                    lista.Add(h);
                }

                respuesta.HistorialActual = lista;
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

        public VTAModVentaOportunidadDetalleDTORPT ObtenerTodasOportunidadesRecordatorio()
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                var now = DateTime.UtcNow;

                var oportunidades = new List<VTAModVentaOportunidadDetalleDTO>();
                var historiales = new List<VTAModVentaTHistorialEstadoDetalleDTO>();

                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SP_ObtenerTodasOportunidadesRecordatorio";

                if (conn.State != ConnectionState.Open) conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int opId = reader.IsDBNull(reader.GetOrdinal("OportunidadId")) ? 0 : reader.GetInt32(reader.GetOrdinal("OportunidadId"));
                    int? idPotencial = reader.IsDBNull(reader.GetOrdinal("IdPotencialCliente")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdPotencialCliente"));

                    string personaNombres = reader.IsDBNull(reader.GetOrdinal("Persona_Nombres")) ? string.Empty : reader.GetString(reader.GetOrdinal("Persona_Nombres"));
                    string personaApellidos = reader.IsDBNull(reader.GetOrdinal("Persona_Apellidos")) ? string.Empty : reader.GetString(reader.GetOrdinal("Persona_Apellidos"));
                    string personaCorreo = reader.IsDBNull(reader.GetOrdinal("Persona_Correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Persona_Correo"));

                    int? personaPaisId = reader.IsDBNull(reader.GetOrdinal("Persona_PaisId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Persona_PaisId"));
                    string personaPaisNombre = reader.IsDBNull(reader.GetOrdinal("Persona_PaisNombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Persona_PaisNombre"));

                    int idProducto = reader.IsDBNull(reader.GetOrdinal("IdProducto")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdProducto"));
                    string productoNombre = reader.IsDBNull(reader.GetOrdinal("Producto_Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto_Nombre"));
                    string codigoLanzamiento = reader.IsDBNull(reader.GetOrdinal("CodigoLanzamiento")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoLanzamiento"));
                    string origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? string.Empty : reader.GetString(reader.GetOrdinal("Origen"));
                    bool estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? true : reader.GetBoolean(reader.GetOrdinal("Estado"));
                    DateTime fechaCreacion = reader.IsDBNull(reader.GetOrdinal("Oportunidad_FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Oportunidad_FechaCreacion"));
                    string usuarioCreacion = reader.IsDBNull(reader.GetOrdinal("Oportunidad_UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Oportunidad_UsuarioCreacion"));

                    int totalOportunidadesPersona = reader.IsDBNull(reader.GetOrdinal("TotalOportunidadesPersona")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalOportunidadesPersona"));

                    var dto = new VTAModVentaOportunidadDetalleDTO
                    {
                        Id = opId,
                        IdPotencialCliente = idPotencial ?? 0,
                        PersonaNombre = $"{personaNombres} {personaApellidos}".Trim(),
                        PersonaCorreo = personaCorreo ?? string.Empty,
                        IdProducto = idProducto,
                        ProductoNombre = productoNombre ?? string.Empty,
                        CodigoLanzamiento = codigoLanzamiento ?? string.Empty,
                        PersonaPaisId = personaPaisId,
                        PersonaPaisNombre = personaPaisNombre ?? string.Empty,
                        Origen = origen,
                        Estado = estado,
                        TotalOportunidadesPersona = totalOportunidadesPersona,
                        FechaCreacion = fechaCreacion,
                        UsuarioCreacion = usuarioCreacion ?? string.Empty
                    };

                    if (!reader.IsDBNull(reader.GetOrdinal("UltimoHist_Id")))
                    {
                        var uhId = reader.GetInt32(reader.GetOrdinal("UltimoHist_Id"));
                        dto.IdHistorialEstado = uhId;
                        dto.IdEstado = reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UltimoHist_IdEstado"));

                        dto.NombreEstado = reader.IsDBNull(reader.GetOrdinal("UltimoHist_NombreEstado"))
                            ? string.Empty
                            : reader.GetString(reader.GetOrdinal("UltimoHist_NombreEstado"));
                        int? ultimoHist_IdOcurrencia = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdOcurrencia")))
                            ultimoHist_IdOcurrencia = reader.GetInt32(reader.GetOrdinal("UltimoHist_IdOcurrencia"));
                        if (ultimoHist_IdOcurrencia == null && !reader.IsDBNull(reader.GetOrdinal("UltimoHist_OcurrenciaId")))
                            ultimoHist_IdOcurrencia = reader.GetInt32(reader.GetOrdinal("UltimoHist_OcurrenciaId"));

                        string ultimoHist_OcurrenciaNombre = string.Empty;
                        if (!reader.IsDBNull(reader.GetOrdinal("UltimoHist_OcurrenciaNombre")))
                            ultimoHist_OcurrenciaNombre = reader.GetString(reader.GetOrdinal("UltimoHist_OcurrenciaNombre"));

                        dto.IdOcurrencia = ultimoHist_IdOcurrencia;
                        dto.NombreOcurrencia = !string.IsNullOrWhiteSpace(ultimoHist_OcurrenciaNombre) ? ultimoHist_OcurrenciaNombre : string.Empty;

                        var histDto = new VTAModVentaTHistorialEstadoDetalleDTO
                        {
                            Id = uhId,
                            IdOportunidad = opId,
                            IdAsesor = reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdAsesor")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UltimoHist_IdAsesor")),
                            IdEstado = reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("UltimoHist_IdEstado")),
                            Observaciones = reader.IsDBNull(reader.GetOrdinal("UltimoHist_Observaciones")) ? string.Empty : reader.GetString(reader.GetOrdinal("UltimoHist_Observaciones")),
                            CantidadLlamadasContestadas = reader.IsDBNull(reader.GetOrdinal("UltimoHist_CantidadLlamadasContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("UltimoHist_CantidadLlamadasContestadas")),
                            CantidadLlamadasNoContestadas = reader.IsDBNull(reader.GetOrdinal("UltimoHist_CantidadLlamadasNoContestadas")) ? 0 : reader.GetInt32(reader.GetOrdinal("UltimoHist_CantidadLlamadasNoContestadas")),
                            Estado = true,
                            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("UltimoHist_FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("UltimoHist_FechaCreacion")),
                            UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UltimoHist_UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UltimoHist_UsuarioCreacion"))
                        };

                        histDto.IdOcurrencia = ultimoHist_IdOcurrencia;

                        if (!string.IsNullOrEmpty(ultimoHist_OcurrenciaNombre))
                        {

                            histDto.OcurrenciaNombre = ultimoHist_OcurrenciaNombre;

                        }

                        historiales.Add(histDto);
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("HistInter_Id")))
                    {
                        dto.IdHistorialInteraccion = reader.GetInt32(reader.GetOrdinal("HistInter_Id"));
                        var fr = reader.IsDBNull(reader.GetOrdinal("HistInter_FechaRecordatorio")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("HistInter_FechaRecordatorio"));
                        dto.FechaRecordatorio = fr ?? (DateTime?)null;
                    }

                    oportunidades.Add(dto);
                }

                respuesta.Oportunidad = oportunidades;
                respuesta.HistorialActual = historiales;
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


        public VTAModVentaOportunidadDetalleDTO ObtenerOportunidadPorIdConRecordatorio(int id)
        {
            try
            {
                var now = DateTime.UtcNow;

                var x = _unitOfWork.OportunidadRepository.ObtenerTodas()
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        o.IdPotencialCliente,
                        o.PotencialCliente,
                        PersonaNombres = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Nombres : null,
                        PersonaApellidos = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Apellidos : null,
                        IdPais = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.IdPais : null,
                        PaisNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null && o.PotencialCliente.Persona.Pais != null ? o.PotencialCliente.Persona.Pais.Nombre : null,
                        CorreoPersona = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Correo : null,
                        o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : null,
                        o.CodigoLanzamiento,
                        o.Estado,
                        o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        o.FechaModificacion,
                        o.UsuarioModificacion,

                        // Último historialEstado
                        UltimoHistorial = o.HistorialEstado
                            .OrderByDescending(h => h.FechaCreacion)
                            .ThenByDescending(h => h.Id)
                            .Select(h => new {
                                h.Id,
                                h.IdEstado,
                                NombreEstado = h.EstadoReferencia != null ? h.EstadoReferencia.Nombre : null
                            })
                            .FirstOrDefault(),

                        // HistorialInteraccion tipo 10
                        HistorialInteraccionTipo10 = o.HistorialInteracciones
                            .Where(hi => hi.IdTipo == 10)
                            .OrderBy(hi => hi.FechaRecordatorio == null ? DateTime.MaxValue : hi.FechaRecordatorio)
                            .ThenByDescending(hi => hi.FechaCreacion)
                            .Select(hi => new { hi.Id, hi.FechaRecordatorio })
                            .FirstOrDefault(),

                        // Calcular TotalOportunidadesPersona
                        TotalOportunidadesPersona = _unitOfWork.OportunidadRepository
                            .Query()
                            .Count(otherO => otherO.IdPotencialCliente == o.IdPotencialCliente)
                    })
                    .FirstOrDefault();

                if (x == null) return new VTAModVentaOportunidadDetalleDTO();

                var dto = new VTAModVentaOportunidadDetalleDTO
                {
                    Id = x.Id,
                    IdPotencialCliente = x.IdPotencialCliente, // Cambiado de IdPersona
                    PersonaNombre = $"{(x.PersonaNombres ?? "")} {(x.PersonaApellidos ?? "")}".Trim(),
                    IdProducto = x.IdProducto,
                    ProductoNombre = x.ProductoNombre ?? string.Empty,
                    PersonaCorreo = x.CorreoPersona ?? string.Empty,
                    CodigoLanzamiento = x.CodigoLanzamiento,
                    Estado = x.Estado,
                    TotalOportunidadesPersona = x.TotalOportunidadesPersona
                };

                if (x.UltimoHistorial != null)
                {
                    dto.IdHistorialEstado = x.UltimoHistorial.Id;
                    dto.IdEstado = x.UltimoHistorial.IdEstado;
                    dto.NombreEstado = x.UltimoHistorial.NombreEstado ?? string.Empty;
                }

                if (x.HistorialInteraccionTipo10 != null)
                {
                    dto.IdHistorialInteraccion = x.HistorialInteraccionTipo10.Id;
                    var fr = x.HistorialInteraccionTipo10.FechaRecordatorio;
                    dto.FechaRecordatorio = (fr.HasValue && fr.Value.ToUniversalTime() >= now) ? fr.Value : (DateTime?)null;
                }

                return dto;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new VTAModVentaOportunidadDetalleDTO();
            }
        }

        public VTAModVentaPotencialClienteDTO ObtenerPotencialPorOportunidadId(int idOportunidad)
        {
            var dto = new VTAModVentaPotencialClienteDTO();

            try
            {
                var resultado = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Where(o => o.Id == idOportunidad)
                    .Select(o => o.PotencialCliente)
                    .Where(pc => pc != null)
                    .Select(pc => new VTAModVentaPotencialClienteDTO
                    {
                        Id = pc.Id,
                        IdPersona = pc.IdPersona,
                        Desuscrito = pc.Desuscrito,
                        Estado = pc.Estado,
                        FechaCreacion = pc.FechaCreacion,
                        UsuarioCreacion = pc.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = pc.FechaModificacion,
                        UsuarioModificacion = pc.UsuarioModificacion ?? string.Empty,

                        Persona = pc.Persona == null ? null : new VTAModVentaTPersonaDTO
                        {
                            Id = pc.Persona.Id,
                            IdPais = pc.Persona.IdPais,
                            Pais = pc.Persona.Pais != null ? pc.Persona.Pais.Nombre : string.Empty,
                            Nombres = pc.Persona.Nombres ?? string.Empty,
                            Apellidos = pc.Persona.Apellidos ?? string.Empty,
                            Celular = pc.Persona.Celular ?? string.Empty,
                            PrefijoPaisCelular = pc.Persona.PrefijoPaisCelular ?? string.Empty,
                            Correo = pc.Persona.Correo ?? string.Empty,
                            AreaTrabajo = pc.Persona.AreaTrabajo ?? string.Empty,
                            Industria = pc.Persona.Industria ?? string.Empty,
                            Estado = pc.Persona.Estado,
                            FechaCreacion = pc.Persona.FechaCreacion,
                            UsuarioCreacion = pc.Persona.UsuarioCreacion ?? string.Empty,
                            FechaModificacion = (DateTime?)pc.Persona.FechaModificacion,
                            UsuarioModificacion = pc.Persona.UsuarioModificacion ?? string.Empty
                        }
                    })
                    .FirstOrDefault();

                if (resultado != null)
                    dto = resultado;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }

            return dto;
        }

        public CFGRespuestaGenericaDTO InsertarOportunidadHistorialRegistrado(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

                Producto? producto = null;
                if (dto.IdProducto.HasValue)
                {
                    producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (producto == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                if (dto.FechaRecordatorio == default(DateTime) || string.IsNullOrWhiteSpace(dto.HoraRecordatorio))
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "FechaRecordatorio y HoraRecordatorio no pueden ser nulos.";
                    return respuesta;
                }

                string usuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion;
                string usuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;
                DateTime nowUtc() => DateTime.UtcNow;

                DateTime BuildFechaRecordatorio()
                {
                    var fecha = dto.FechaRecordatorio; // ya validado no-default
                    var horaStr = dto.HoraRecordatorio ?? string.Empty;

                    if (TimeSpan.TryParse(horaStr, out var ts))
                    {
                        return fecha.Date.Add(ts);
                    }

                    var formatos = new[] { "HH:mm", "H:mm", "HH:mm:ss" };
                    if (DateTime.TryParseExact(horaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtHoraExact))
                    {
                        return fecha.Date.Add(dtHoraExact.TimeOfDay);
                    }

                    if (DateTime.TryParse(horaStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtAny))
                    {
                        return fecha.Date.Add(dtAny.TimeOfDay);
                    }

                    throw new ArgumentException("HoraRecordatorio no tiene un formato válido. Use 'HH:mm' o 'HH:mm:ss'.");
                }

                DateTime fechaRecordatorio;
                try
                {
                    fechaRecordatorio = BuildFechaRecordatorio();
                }
                catch (ArgumentException aex)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = aex.Message;
                    return respuesta;
                }

                var ent = new Oportunidad
                {
                    IdPotencialCliente = dto.IdPotencialCliente,
                    IdProducto = dto.IdProducto,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Origen = dto.Origen,
                    Estado = dto.Estado,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                var historialEstado = new HistorialEstado
                {
                    IdAsesor = 1,
                    IdEstado = 1,
                    IdOcurrencia = 1,
                    Observaciones = "Estado Inicial",
                    CantidadLlamadasContestadas = 0,
                    CantidadLlamadasNoContestadas = 0,
                    Estado = true,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                var historialInteraccion = new HistorialInteraccion
                {
                    IdTipo = 10,
                    Detalle = "Recordatorio inicial de creación de la oportunidad, generado automaticamente",
                    Celular = null,
                    FechaRecordatorio = fechaRecordatorio,
                    Estado = true,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                DbContext? dbContext = null;
                var uwType = _unitOfWork.GetType();
                var propCandidates = new[] { "Context", "_context", "DbContext", "Contexto", "ContextoDb" };
                foreach (var pName in propCandidates)
                {
                    var prop = uwType.GetProperty(pName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (prop != null)
                    {
                        dbContext = prop.GetValue(_unitOfWork) as DbContext;
                        if (dbContext != null) break;
                    }
                }

                if (dbContext != null)
                {
                    using (var tx = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _unitOfWork.OportunidadRepository.Insertar(ent);
                            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                            historialEstado.IdOportunidad = ent.Id;
                            historialInteraccion.IdOportunidad = ent.Id;

                            _unitOfWork.HistorialEstadoRepository.Insertar(historialEstado);
                            _unitOfWork.HistorialInteraccionRepository.Insertar(historialInteraccion);
                            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                            if (producto != null && ent.IdProducto.HasValue)
                            {
                                decimal costoTotal = producto.CostoBase ?? 0m;
                                var inversion = new Inversion
                                {
                                    IdProducto = ent.IdProducto.Value,
                                    IdOportunidad = ent.Id,
                                    CostoTotal = costoTotal,
                                    Moneda = "USD",
                                    DescuentoPorcentaje = null,
                                    CostoOfrecido = costoTotal,
                                    Estado = true,
                                    IdMigracion = null,
                                    FechaCreacion = nowUtc(),
                                    UsuarioCreacion = usuarioCreacion,
                                    FechaModificacion = nowUtc(),
                                    UsuarioModificacion = usuarioModificacion
                                };

                                _unitOfWork.InversionRepository.Insertar(inversion);
                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                            }

                            tx.Commit();

                            respuesta.Codigo = SR._C_SIN_ERROR;
                            respuesta.Mensaje = string.Empty;
                        }
                        catch (Exception)
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
                else
                {
                    _unitOfWork.OportunidadRepository.Insertar(ent);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                    historialEstado.IdOportunidad = ent.Id;
                    historialInteraccion.IdOportunidad = ent.Id;

                    _unitOfWork.HistorialEstadoRepository.Insertar(historialEstado);
                    _unitOfWork.HistorialInteraccionRepository.Insertar(historialInteraccion);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                    if (producto != null && ent.IdProducto.HasValue)
                    {
                        decimal costoTotal = producto.CostoBase ?? 0m;
                        var inversion = new Inversion
                        {
                            IdProducto = ent.IdProducto.Value,
                            IdOportunidad = ent.Id,
                            CostoTotal = costoTotal,
                            Moneda = "USD",
                            DescuentoPorcentaje = null,
                            CostoOfrecido = costoTotal,
                            Estado = true,
                            IdMigracion = null,
                            FechaCreacion = nowUtc(),
                            UsuarioCreacion = usuarioCreacion,
                            FechaModificacion = nowUtc(),
                            UsuarioModificacion = usuarioModificacion
                        };

                        _unitOfWork.InversionRepository.Insertar(inversion);
                        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                    }

                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.Empty;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public VTAModVentaOportunidadDetalleDTORPT ObtenerTodasOportunidadesRecordatorio2(int idUsuario, int idRol)
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                var oportunidades = new List<VTAModVentaOportunidadDetalleDTO>();
                var historiales = new List<VTAModVentaTHistorialEstadoDetalleDTO>();

                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SP_ObtenerTodasOportunidadesRecordatorio2";

                cmd.Parameters.Add(new SqlParameter("@IdUsuario", idUsuario));
                cmd.Parameters.Add(new SqlParameter("@IdRol", idRol));

                if (conn.State != ConnectionState.Open) conn.Open();
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int opId = reader.GetInt32(reader.GetOrdinal("OportunidadId"));
                    int? idPotencial = reader.IsDBNull(reader.GetOrdinal("IdPotencialCliente"))
                        ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdPotencialCliente"));

                    // Datos Persona
                    string personaNombres = reader.IsDBNull(reader.GetOrdinal("Persona_Nombres")) ? "" : reader.GetString(reader.GetOrdinal("Persona_Nombres"));
                    string personaApellidos = reader.IsDBNull(reader.GetOrdinal("Persona_Apellidos")) ? "" : reader.GetString(reader.GetOrdinal("Persona_Apellidos"));
                    string personaCorreo = reader.IsDBNull(reader.GetOrdinal("Persona_Correo")) ? "" : reader.GetString(reader.GetOrdinal("Persona_Correo"));

                    // 🔥 AGREGADO DEL MÉTODO 1 → País
                    int? personaPaisId = reader.IsDBNull(reader.GetOrdinal("Persona_PaisId"))
                        ? (int?)null
                        : reader.GetInt32(reader.GetOrdinal("Persona_PaisId"));

                    string personaPaisNombre = reader.IsDBNull(reader.GetOrdinal("Persona_PaisNombre"))
                        ? ""
                        : reader.GetString(reader.GetOrdinal("Persona_PaisNombre"));

                    // Datos Asesor
                    int? idAsesor = reader.IsDBNull(reader.GetOrdinal("UltimoHist_Asesor_Id"))
                        ? (int?)null : reader.GetInt32(reader.GetOrdinal("UltimoHist_Asesor_Id"));

                    string asesorNombres = reader.IsDBNull(reader.GetOrdinal("UltimoHist_Asesor_Nombres"))
                        ? "" : reader.GetString(reader.GetOrdinal("UltimoHist_Asesor_Nombres"));

                    string asesorApellidos = reader.IsDBNull(reader.GetOrdinal("UltimoHist_Asesor_Apellidos"))
                        ? "" : reader.GetString(reader.GetOrdinal("UltimoHist_Asesor_Apellidos"));

                    string asesorNombreCompleto = idAsesor.HasValue
                        ? $"{asesorNombres} {asesorApellidos}".Trim()
                        : "SIN ASESOR";

                    int idProducto = reader.IsDBNull(reader.GetOrdinal("IdProducto"))
                        ? 0 : reader.GetInt32(reader.GetOrdinal("IdProducto"));

                    string productoNombre = reader.IsDBNull(reader.GetOrdinal("Producto_Nombre")) ? "" : reader.GetString(reader.GetOrdinal("Producto_Nombre"));
                    string codigoLanzamiento = reader.IsDBNull(reader.GetOrdinal("CodigoLanzamiento")) ? "" : reader.GetString(reader.GetOrdinal("CodigoLanzamiento"));
                    string origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? "" : reader.GetString(reader.GetOrdinal("Origen"));
                    bool estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? true : reader.GetBoolean(reader.GetOrdinal("Estado"));
                    DateTime fechaCreacion = reader.IsDBNull(reader.GetOrdinal("Oportunidad_FechaCreacion"))
                        ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("Oportunidad_FechaCreacion"));

                    string usuarioCreacion = reader.IsDBNull(reader.GetOrdinal("Oportunidad_UsuarioCreacion"))
                        ? "" : reader.GetString(reader.GetOrdinal("Oportunidad_UsuarioCreacion"));

                    int totalOportunidadesPersona = reader.IsDBNull(reader.GetOrdinal("TotalOportunidadesPersona"))
                        ? 0 : reader.GetInt32(reader.GetOrdinal("TotalOportunidadesPersona"));

                    var dto = new VTAModVentaOportunidadDetalleDTO
                    {
                        Id = opId,
                        IdPotencialCliente = idPotencial ?? 0,
                        PersonaNombre = $"{personaNombres} {personaApellidos}".Trim(),
                        PersonaCorreo = personaCorreo,

                        // Asesor
                        AsesorNombre = asesorNombreCompleto,
                        IdAsesor = idAsesor,

                        IdProducto = idProducto,
                        ProductoNombre = productoNombre,
                        CodigoLanzamiento = codigoLanzamiento,
                        Origen = origen,
                        Estado = estado,
                        TotalOportunidadesPersona = totalOportunidadesPersona,
                        FechaCreacion = fechaCreacion,
                        UsuarioCreacion = usuarioCreacion,

                        // 🔥 AGREGADO DEL MÉTODO 1 → País
                        PersonaPaisId = personaPaisId,
                        PersonaPaisNombre = personaPaisNombre
                    };

                    // HISTORIAL ESTADO
                    if (!reader.IsDBNull(reader.GetOrdinal("UltimoHist_Id")))
                    {
                        var uhId = reader.GetInt32(reader.GetOrdinal("UltimoHist_Id"));

                        dto.IdHistorialEstado = uhId;
                        dto.IdEstado = reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdEstado"))
                            ? (int?)null
                            : reader.GetInt32(reader.GetOrdinal("UltimoHist_IdEstado"));

                        dto.NombreEstado = reader.IsDBNull(reader.GetOrdinal("UltimoHist_NombreEstado"))
                            ? "" : reader.GetString(reader.GetOrdinal("UltimoHist_NombreEstado"));

                        // 🔥 AGREGADO DEL MÉTODO 1 → Ocurrencias
                        int? idOcurrencia = null;

                        if (!reader.IsDBNull(reader.GetOrdinal("UltimoHist_IdOcurrencia")))
                            idOcurrencia = reader.GetInt32(reader.GetOrdinal("UltimoHist_IdOcurrencia"));

                        if (idOcurrencia == null && !reader.IsDBNull(reader.GetOrdinal("UltimoHist_OcurrenciaId")))
                            idOcurrencia = reader.GetInt32(reader.GetOrdinal("UltimoHist_OcurrenciaId"));

                        string ocurrenciaNombre = reader.IsDBNull(reader.GetOrdinal("UltimoHist_OcurrenciaNombre"))
                            ? ""
                            : reader.GetString(reader.GetOrdinal("UltimoHist_OcurrenciaNombre"));

                        dto.IdOcurrencia = idOcurrencia;
                        dto.NombreOcurrencia = ocurrenciaNombre;

                        var histDto = new VTAModVentaTHistorialEstadoDetalleDTO
                        {
                            Id = uhId,
                            IdOportunidad = opId,
                            IdAsesor = idAsesor,
                            IdEstado = dto.IdEstado,
                            Observaciones = reader.IsDBNull(reader.GetOrdinal("UltimoHist_Observaciones"))
                                ? "" : reader.GetString(reader.GetOrdinal("UltimoHist_Observaciones")),
                            CantidadLlamadasContestadas = reader.IsDBNull(reader.GetOrdinal("UltimoHist_CantidadLlamadasContestadas"))
                                ? 0 : reader.GetInt32(reader.GetOrdinal("UltimoHist_CantidadLlamadasContestadas")),
                            CantidadLlamadasNoContestadas = reader.IsDBNull(reader.GetOrdinal("UltimoHist_CantidadLlamadasNoContestadas"))
                                ? 0 : reader.GetInt32(reader.GetOrdinal("UltimoHist_CantidadLlamadasNoContestadas")),
                            Estado = true,
                            FechaCreacion = reader.IsDBNull(reader.GetOrdinal("UltimoHist_FechaCreacion"))
                                ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("UltimoHist_FechaCreacion")),
                            UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UltimoHist_UsuarioCreacion"))
                                ? "" : reader.GetString(reader.GetOrdinal("UltimoHist_UsuarioCreacion")),

                            IdOcurrencia = idOcurrencia,
                            OcurrenciaNombre = ocurrenciaNombre
                        };

                        historiales.Add(histDto);
                    }

                    // HISTORIAL INTERACCIÓN
                    if (!reader.IsDBNull(reader.GetOrdinal("HistInter_Id")))
                    {
                        dto.IdHistorialInteraccion = reader.GetInt32(reader.GetOrdinal("HistInter_Id"));
                        dto.FechaRecordatorio = reader.IsDBNull(reader.GetOrdinal("HistInter_FechaRecordatorio"))
                            ? null : reader.GetDateTime(reader.GetOrdinal("HistInter_FechaRecordatorio"));
                    }

                    oportunidades.Add(dto);
                }

                respuesta.Oportunidad = oportunidades;
                respuesta.HistorialActual = historiales;
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "";
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return respuesta;
        }

        public CFGRespuestaGenericaDTO AsignarAsesor(VTAModVentaAsignarAsesorDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                foreach (var idOportunidad in dto.IdOportunidades)
                {
                    var oportunidad = _unitOfWork.OportunidadRepository
                        .Query()
                        .FirstOrDefault(i => i.Id == idOportunidad);

                    if (oportunidad != null)
                    {
                        oportunidad.IdPersona = dto.IdAsesor;
                        oportunidad.FechaModificacion = DateTime.UtcNow;
                        oportunidad.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion)
                            ? "SYSTEM"
                            : dto.UsuarioModificacion;

                        _unitOfWork.OportunidadRepository.Actualizar(oportunidad);
                    }
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
