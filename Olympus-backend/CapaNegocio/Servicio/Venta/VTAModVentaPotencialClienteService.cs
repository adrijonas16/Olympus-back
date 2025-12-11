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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaPotencialClienteService : IVTAModVentaPotencialClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaPotencialClienteService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaPotencialClienteDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaPotencialClienteDTORPT();
            try
            {
                var lista = _unitOfWork.PotencialClienteRepository
                    .ObtenerTodos() // IQueryable<PotencialCliente>
                    .Include(p => p.Persona)
                    .AsNoTracking()
                    .Select(p => new VTAModVentaPotencialClienteDTO
                    {
                        Id = p.Id,
                        IdPersona = p.IdPersona,
                        Desuscrito = p.Desuscrito,
                        Estado = p.Estado,
                        FechaCreacion = p.FechaCreacion,
                        UsuarioCreacion = p.UsuarioCreacion,
                        FechaModificacion = p.FechaModificacion,
                        UsuarioModificacion = p.UsuarioModificacion,
                        Persona = p.Persona == null ? null : new VTAModVentaTPersonaDTO
                        {
                            Id = p.Persona.Id,
                            IdPais = p.Persona.IdPais,
                            Pais = p.Persona.Pais != null ? p.Persona.Pais.Nombre : string.Empty,
                            Nombres = p.Persona.Nombres ?? string.Empty,
                            Apellidos = p.Persona.Apellidos ?? string.Empty,
                            Celular = p.Persona.Celular ?? string.Empty,
                            PrefijoPaisCelular = p.Persona.PrefijoPaisCelular ?? string.Empty,
                            Correo = p.Persona.Correo ?? string.Empty,
                            AreaTrabajo = p.Persona.AreaTrabajo ?? string.Empty,
                            Industria = p.Persona.Industria ?? string.Empty,
                            Estado = p.Persona.Estado,
                            FechaCreacion = p.Persona.FechaCreacion,
                            UsuarioCreacion = p.Persona.UsuarioCreacion ?? string.Empty,
                            FechaModificacion = p.Persona.FechaModificacion,
                            UsuarioModificacion = p.Persona.UsuarioModificacion
                        }
                    })
                    .ToList();

                respuesta.PotencialClientes = lista;
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

        public VTAModVentaPotencialClienteDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaPotencialClienteDTO();
            try
            {
                var ent = _unitOfWork.PotencialClienteRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdPersona = ent.IdPersona;
                    dto.Desuscrito = ent.Desuscrito;
                    dto.Estado = ent.Estado;
                    dto.FechaCreacion = ent.FechaCreacion;
                    dto.UsuarioCreacion = ent.UsuarioCreacion;
                    dto.FechaModificacion = ent.FechaModificacion;
                    dto.UsuarioModificacion = ent.UsuarioModificacion;

                    if (ent.Persona != null)
                    {
                        dto.Persona = new VTAModVentaTPersonaDTO
                        {
                            Id = ent.Persona.Id,
                            IdPais = ent.Persona.IdPais,
                            Pais = ent.Persona.Pais != null ? ent.Persona.Pais.Nombre : string.Empty,
                            Nombres = ent.Persona.Nombres ?? string.Empty,
                            Apellidos = ent.Persona.Apellidos ?? string.Empty,
                            Celular = ent.Persona.Celular ?? string.Empty,
                            PrefijoPaisCelular = ent.Persona.PrefijoPaisCelular ?? string.Empty,
                            Correo = ent.Persona.Correo ?? string.Empty,
                            AreaTrabajo = ent.Persona.AreaTrabajo ?? string.Empty,
                            Industria = ent.Persona.Industria ?? string.Empty,
                            Estado = ent.Persona.Estado,
                            FechaCreacion = ent.Persona.FechaCreacion,
                            UsuarioCreacion = ent.Persona.UsuarioCreacion ?? string.Empty,
                            FechaModificacion = ent.Persona.FechaModificacion,
                            UsuarioModificacion = ent.Persona.UsuarioModificacion
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public VTAModVentaPotencialClienteInsertRPT Insertar(VTAModVentaPotencialClienteDTO dto)
        {
            var respuesta = new VTAModVentaPotencialClienteInsertRPT();
            try
            {
                if (dto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "DTO inválido.";
                    return respuesta;
                }

                string usuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion;
                string usuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;
                DateTime nowUtc() => DateTime.UtcNow;

                int idPersonaFinal = 0;
                int idPotencialFinal = 0;

                var dbContext = _unitOfWork.Context as DbContext;

                if (dto.Persona != null)
                {
                    var pDto = dto.Persona;
                    var personaEnt = new Persona
                    {
                        IdPais = pDto.IdPais,
                        Nombres = pDto.Nombres ?? string.Empty,
                        Apellidos = pDto.Apellidos ?? string.Empty,
                        Celular = pDto.Celular ?? string.Empty,
                        PrefijoPaisCelular = pDto.PrefijoPaisCelular ?? string.Empty,
                        Correo = pDto.Correo ?? string.Empty,
                        AreaTrabajo = pDto.AreaTrabajo ?? string.Empty,
                        Industria = pDto.Industria ?? string.Empty,
                        Estado = pDto.Estado,
                        FechaCreacion = nowUtc(),
                        UsuarioCreacion = string.IsNullOrWhiteSpace(pDto.UsuarioCreacion) ? usuarioCreacion : pDto.UsuarioCreacion,
                        FechaModificacion = pDto.FechaModificacion ?? nowUtc(),
                        UsuarioModificacion = string.IsNullOrWhiteSpace(pDto.UsuarioModificacion) ? usuarioModificacion : pDto.UsuarioModificacion
                    };

                    if (dbContext != null)
                    {
                        using (var tx = dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                _unitOfWork.PersonaRepository.Insertar(personaEnt);
                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                                idPersonaFinal = personaEnt.Id;

                                var potencial = new PotencialCliente
                                {
                                    IdPersona = idPersonaFinal,
                                    Desuscrito = dto.Desuscrito,
                                    Estado = dto.Estado,
                                    FechaCreacion = nowUtc(),
                                    UsuarioCreacion = usuarioCreacion,
                                    FechaModificacion = nowUtc(),
                                    UsuarioModificacion = usuarioModificacion
                                };

                                _unitOfWork.PotencialClienteRepository.Insertar(potencial);
                                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                                idPotencialFinal = potencial.Id;

                                tx.Commit();

                                respuesta.IdPersona = idPersonaFinal;
                                respuesta.IdPotencialCliente = idPotencialFinal;
                                respuesta.Codigo = SR._C_SIN_ERROR;
                                respuesta.Mensaje = string.Empty;
                                return respuesta;
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
                        // Sin DbContext transaccional (caída al comportamiento anterior)
                        _unitOfWork.PersonaRepository.Insertar(personaEnt);
                        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                        idPersonaFinal = personaEnt.Id;

                        var potencial = new PotencialCliente
                        {
                            IdPersona = idPersonaFinal,
                            Desuscrito = dto.Desuscrito,
                            Estado = dto.Estado,
                            FechaCreacion = nowUtc(),
                            UsuarioCreacion = usuarioCreacion,
                            FechaModificacion = nowUtc(),
                            UsuarioModificacion = usuarioModificacion
                        };

                        _unitOfWork.PotencialClienteRepository.Insertar(potencial);
                        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                        idPotencialFinal = potencial.Id;

                        respuesta.IdPersona = idPersonaFinal;
                        respuesta.IdPotencialCliente = idPotencialFinal;
                        respuesta.Codigo = SR._C_SIN_ERROR;
                        respuesta.Mensaje = string.Empty;
                        return respuesta;
                    }
                }
                else if (dto.IdPersona > 0)
                {
                    var existing = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                    if (existing == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "IdPersona proporcionado no existe.";
                        return respuesta;
                    }

                    idPersonaFinal = dto.IdPersona;

                    var potencial = new PotencialCliente
                    {
                        IdPersona = idPersonaFinal,
                        Desuscrito = dto.Desuscrito,
                        Estado = dto.Estado,
                        FechaCreacion = nowUtc(),
                        UsuarioCreacion = usuarioCreacion,
                        FechaModificacion = nowUtc(),
                        UsuarioModificacion = usuarioModificacion
                    };

                    _unitOfWork.PotencialClienteRepository.Insertar(potencial);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                    idPotencialFinal = potencial.Id;

                    respuesta.IdPersona = idPersonaFinal;
                    respuesta.IdPotencialCliente = idPotencialFinal;
                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.Empty;
                    return respuesta;
                }
                else
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Debe enviar dto.Persona para crear la persona o dto.IdPersona para usar una existente.";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new VTAModVentaPotencialClienteInsertRPT
                {
                    Codigo = SR._C_ERROR_CRITICO,
                    Mensaje = ex.Message,
                    IdPersona = 0,
                    IdPotencialCliente = 0
                };
            }
        }

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaPotencialClienteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                if (dto == null || dto.Id <= 0)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "DTO inválido o Id faltante.";
                    return respuesta;
                }

                var ent = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                string usuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;
                DateTime nowUtc() => DateTime.UtcNow;

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

                void AplicarCambiosPersona(Persona personaEnt, VTAModVentaTPersonaDTO pDto)
                {
                    if (personaEnt == null || pDto == null) return;

                    personaEnt.IdPais = pDto.IdPais;
                    personaEnt.Nombres = pDto.Nombres ?? string.Empty;
                    personaEnt.Apellidos = pDto.Apellidos ?? string.Empty;
                    personaEnt.Celular = pDto.Celular ?? string.Empty;
                    personaEnt.PrefijoPaisCelular = pDto.PrefijoPaisCelular ?? string.Empty;
                    personaEnt.Correo = pDto.Correo ?? string.Empty;
                    personaEnt.AreaTrabajo = pDto.AreaTrabajo ?? string.Empty;
                    personaEnt.Industria = pDto.Industria ?? string.Empty;
                    personaEnt.Estado = pDto.Estado;
                    personaEnt.FechaModificacion = pDto.FechaModificacion ?? nowUtc();
                    personaEnt.UsuarioModificacion = string.IsNullOrWhiteSpace(pDto.UsuarioModificacion) ? usuarioModificacion : pDto.UsuarioModificacion;
                }

                if (dbContext != null)
                {
                    using (var tx = dbContext.Database.BeginTransaction())
                    {
                        try
                        {

                            int personaIdDesdePersonaDto = dto.Persona?.Id ?? 0;
                            int personaIdDesdeDto = dto.IdPersona;
                            int personaIdAUsar = personaIdDesdePersonaDto > 0 ? personaIdDesdePersonaDto
                                                  : (personaIdDesdeDto > 0 ? personaIdDesdeDto : ent.IdPersona);

                            if (dto.Persona != null)
                            {
                                var personaEnt = _unitOfWork.PersonaRepository.ObtenerPorId(personaIdAUsar);
                                if (personaEnt == null)
                                {
                                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                                    respuesta.Mensaje = "La persona indicada no existe.";
                                    return respuesta;
                                }

                                AplicarCambiosPersona(personaEnt, dto.Persona);
                                _unitOfWork.PersonaRepository.Actualizar(personaEnt);
                            }
                            else if (dto.IdPersona > 0 && dto.IdPersona != ent.IdPersona)
                            {
                                var nuevaPersona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                                if (nuevaPersona == null)
                                {
                                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                                    respuesta.Mensaje = "La persona indicada no existe.";
                                    return respuesta;
                                }
                                ent.IdPersona = dto.IdPersona;
                            }
                            ent.Desuscrito = dto.Desuscrito;
                            ent.Estado = dto.Estado;
                            ent.FechaModificacion = nowUtc();
                            ent.UsuarioModificacion = usuarioModificacion;

                            _unitOfWork.PotencialClienteRepository.Actualizar(ent);
                            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                            tx.Commit();

                            respuesta.Codigo = SR._C_SIN_ERROR;
                            respuesta.Mensaje = string.Empty;
                            return respuesta;
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
                    int personaIdDesdePersonaDto = dto.Persona?.Id ?? 0;
                    int personaIdDesdeDto = dto.IdPersona;
                    int personaIdAUsar = personaIdDesdePersonaDto > 0 ? personaIdDesdePersonaDto
                                              : (personaIdDesdeDto > 0 ? personaIdDesdeDto : ent.IdPersona);

                    if (dto.Persona != null)
                    {
                        var personaEnt = _unitOfWork.PersonaRepository.ObtenerPorId(personaIdAUsar);
                        if (personaEnt == null)
                        {
                            respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                            respuesta.Mensaje = "La persona indicada no existe.";
                            return respuesta;
                        }
                        AplicarCambiosPersona(personaEnt, dto.Persona);
                        _unitOfWork.PersonaRepository.Actualizar(personaEnt);
                    }
                    else if (dto.IdPersona > 0 && dto.IdPersona != ent.IdPersona)
                    {
                        var nuevaPersona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                        if (nuevaPersona == null)
                        {
                            respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                            respuesta.Mensaje = "La persona indicada no existe.";
                            return respuesta;
                        }
                        ent.IdPersona = dto.IdPersona;
                    }

                    ent.Desuscrito = dto.Desuscrito;
                    ent.Estado = dto.Estado;
                    ent.FechaModificacion = nowUtc();
                    ent.UsuarioModificacion = usuarioModificacion;

                    _unitOfWork.PotencialClienteRepository.Actualizar(ent);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.Empty;
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new CFGRespuestaGenericaDTO
                {
                    Codigo = SR._C_ERROR_CRITICO,
                    Mensaje = ex.Message
                };
            }
        }

        public CFGRespuestaGenericaDTO Eliminar(int id)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var success = _unitOfWork.PotencialClienteRepository.Eliminar(id);
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
        public CFGRespuestaGenericaDTO ImportarProcesadoLinkedin(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var dbContext = _unitOfWork.Context;

                DbConnection maybeConn = dbContext.Database.GetDbConnection();
                SqlConnection? sqlConn = maybeConn as SqlConnection;

                if (sqlConn == null)
                {
                    sqlConn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                }

                if (sqlConn.State != ConnectionState.Open)
                    sqlConn.Open();

                try
                {
                    using var cmd = sqlConn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "adm.SPImportarProcesadoLinkedin";

                    var p1 = new SqlParameter("@FechaInicio", SqlDbType.DateTime)
                    {
                        Value = fechaInicio.HasValue ? (object)fechaInicio.Value : DBNull.Value
                    };
                    var p2 = new SqlParameter("@FechaFin", SqlDbType.DateTime)
                    {
                        Value = fechaFin.HasValue ? (object)fechaFin.Value : DBNull.Value
                    };
                    cmd.Parameters.Add(p1);
                    cmd.Parameters.Add(p2);

                    using var reader = cmd.ExecuteReader();

                    int filas = 0;
                    string mensajeSP = string.Empty;

                    if (reader.Read())
                    {
                        string col0Name = reader.FieldCount > 0 ? reader.GetName(0).ToLowerInvariant() : string.Empty;

                        if (col0Name.Contains("filas") || col0Name.Contains("filasprocesadas") || col0Name.Contains("filasenrango"))
                        {
                            filas = reader.IsDBNull(0) ? 0 : Convert.ToInt32(reader.GetValue(0));
                        }
                        else
                        {
                            var v0 = reader.GetValue(0);
                            if (v0 != null && int.TryParse(v0.ToString(), out var tmp)) filas = tmp;
                            else mensajeSP = v0?.ToString() ?? string.Empty;
                        }

                        if (reader.FieldCount > 1)
                        {
                            try { mensajeSP = reader.IsDBNull(1) ? mensajeSP : (reader.GetValue(1)?.ToString() ?? mensajeSP); }
                            catch { /* ignorar */ }
                        }
                    }
                    else
                    {
                        filas = 0;
                        mensajeSP = "El procedimiento no devolvió filas.";
                    }

                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.IsNullOrWhiteSpace(mensajeSP)
                        ? $"Filas procesadas: {filas}"
                        : $"Filas: {filas}. Mensaje: {mensajeSP}";
                }
                finally
                {
                    if (sqlConn != null && sqlConn.State == ConnectionState.Open)
                    {
                        try { sqlConn.Close(); } catch { /* ignorar */ }
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new CFGRespuestaGenericaDTO
                {
                    Codigo = SR._C_ERROR_CRITICO,
                    Mensaje = ex.Message
                };
            }
        }



    }
}
