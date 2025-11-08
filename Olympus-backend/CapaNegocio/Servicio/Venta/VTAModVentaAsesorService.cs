using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaAsesorService : IVTAModVentaAsesorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaAsesorService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTAsesorDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTAsesorDTORPT();
            try
            {
                var lista = _unitOfWork.AsesorRepository.ObtenerTodos()
                    .Select(a => new VTAModVentaTAsesorDTO
                    {
                        Id = a.Id,
                        IdPais = a.IdPais,
                        Pais = a.Pais != null ? a.Pais.Nombre : string.Empty,
                        Nombres = a.Nombres ?? string.Empty,
                        Apellidos = a.Apellidos ?? string.Empty,
                        Celular = a.Celular ?? string.Empty,
                        PrefijoPaisCelular = a.PrefijoPaisCelular ?? string.Empty,
                        Correo = a.Correo ?? string.Empty,
                        AreaTrabajo = a.AreaTrabajo ?? string.Empty,
                        Cesado = a.Cesado,
                        Estado = a.Estado,
                        UsuarioCreacion = a.UsuarioCreacion,
                        FechaCreacion = a.FechaCreacion,
                        UsuarioModificacion = a.UsuarioModificacion,
                        FechaModificacion = a.FechaModificacion
                    })
                    .ToList();

                respuesta.Asesores = lista;
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

        public VTAModVentaTAsesorDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTAsesorDTO();
            try
            {
                var ent = _unitOfWork.AsesorRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdPais = ent.IdPais;
                    dto.Pais = ent.Pais != null ? ent.Pais.Nombre : string.Empty;
                    dto.Nombres = ent.Nombres ?? string.Empty;
                    dto.Apellidos = ent.Apellidos ?? string.Empty;
                    dto.Celular = ent.Celular ?? string.Empty;
                    dto.PrefijoPaisCelular = ent.PrefijoPaisCelular ?? string.Empty;
                    dto.Correo = ent.Correo ?? string.Empty;
                    dto.AreaTrabajo = ent.AreaTrabajo ?? string.Empty;
                    dto.Cesado = ent.Cesado;
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTAsesorDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                if (dto.IdPais.HasValue)
                {
                    var pais = _unitOfWork.PaisRepository.ObtenerPorId(dto.IdPais.Value);
                    if (pais == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "País no encontrado.";
                        return respuesta;
                    }
                    // Estado del Pais
                }
                var ent = new Asesor
                {
                    IdPais = dto.IdPais,
                    Nombres = dto.Nombres,
                    Apellidos = dto.Apellidos,
                    Celular = dto.Celular,
                    PrefijoPaisCelular = dto.PrefijoPaisCelular,
                    Correo = dto.Correo,
                    AreaTrabajo = dto.AreaTrabajo,
                    Cesado = dto.Cesado,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.AsesorRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTAsesorDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.AsesorRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }
                if (dto.IdPais.HasValue)
                {
                    var pais = _unitOfWork.PaisRepository.ObtenerPorId(dto.IdPais.Value);
                    if (pais == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "País no encontrado.";
                        return respuesta;
                    }
                    // Estado del Pais
                }

                ent.IdPais = dto.IdPais;
                ent.Nombres = dto.Nombres;
                ent.Apellidos = dto.Apellidos;
                ent.Celular = dto.Celular;
                ent.PrefijoPaisCelular = dto.PrefijoPaisCelular;
                ent.Correo = dto.Correo;
                ent.AreaTrabajo = dto.AreaTrabajo;
                ent.Cesado = dto.Cesado;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.AsesorRepository.Actualizar(ent);
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
                var success = _unitOfWork.AsesorRepository.Eliminar(id);
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
