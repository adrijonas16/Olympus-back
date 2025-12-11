using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Linq;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaDocenteService : IVTAModVentaDocenteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaDocenteService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaDocenteDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaDocenteDTORPT();
            try
            {
                var lista = _unitOfWork.DocenteRepository
                    .Query()
                    .AsNoTracking()
                    .Include(d => d.Persona)
                    .Select(d => new VTAModVentaDocenteDTO
                    {
                        Id = d.Id,
                        IdPersona = d.IdPersona,
                        PersonaNombre = d.Persona != null
                            ? $"{d.Persona.Nombres} {d.Persona.Apellidos}"
                            : string.Empty,
                        TituloProfesional = d.TituloProfesional,
                        Especialidad = d.Especialidad,
                        Logros = d.Logros,
                        Estado = d.Estado,
                        FechaCreacion = d.FechaCreacion,
                        UsuarioCreacion = d.UsuarioCreacion,
                        FechaModificacion = d.FechaModificacion,
                        UsuarioModificacion = d.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Docentes = lista;
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

        public VTAModVentaDocenteDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaDocenteDTO();
            try
            {
                var ent = _unitOfWork.DocenteRepository
                    .Query()
                    .AsNoTracking()
                    .Include(d => d.Persona)
                    .Where(d => d.Id == id)
                    .Select(d => new VTAModVentaDocenteDTO
                    {
                        Id = d.Id,
                        IdPersona = d.IdPersona,
                        PersonaNombre = d.Persona != null
                            ? $"{d.Persona.Nombres} {d.Persona.Apellidos}"
                            : string.Empty,
                        TituloProfesional = d.TituloProfesional,
                        Especialidad = d.Especialidad,
                        Logros = d.Logros,
                        Estado = d.Estado,
                        FechaCreacion = d.FechaCreacion,
                        UsuarioCreacion = d.UsuarioCreacion,
                        FechaModificacion = d.FechaModificacion,
                        UsuarioModificacion = d.UsuarioModificacion
                    })
                    .FirstOrDefault();

                if (ent != null)
                    dto = ent;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaDocenteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validar Persona
                var persona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "La persona asociada no existe.";
                    return respuesta;
                }

                var ent = new Docente
                {
                    IdPersona = dto.IdPersona,
                    TituloProfesional = dto.TituloProfesional,
                    Especialidad = dto.Especialidad,
                    Logros = dto.Logros,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.DocenteRepository.Insertar(ent);
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "Docente registrado correctamente.";
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }
            return respuesta;
        }

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaDocenteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.DocenteRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar Persona
                var persona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "La persona asociada no existe.";
                    return respuesta;
                }

                ent.IdPersona = dto.IdPersona;
                ent.TituloProfesional = dto.TituloProfesional;
                ent.Especialidad = dto.Especialidad;
                ent.Logros = dto.Logros;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.DocenteRepository.Actualizar(ent);
                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "Docente actualizado correctamente.";
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
                var success = _unitOfWork.DocenteRepository.Eliminar(id);
                if (!success)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = "Docente eliminado correctamente.";
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
