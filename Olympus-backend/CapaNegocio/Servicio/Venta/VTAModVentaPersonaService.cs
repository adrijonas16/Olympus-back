using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaPersonaService : IVTAModVentaPersonaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaPersonaService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTPersonaDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTPersonaDTORPT();
            try
            {
                var lista = _unitOfWork.PersonaRepository.ObtenerTodos()
                    .Select(p => new VTAModVentaTPersonaDTO
                    {
                        Id = p.Id,
                        IdPais = p.IdPais,
                        Nombres = p.Nombres,
                        Apellidos = p.Apellidos,
                        Celular = p.Celular,
                        PrefijoPaisCelular = p.PrefijoPaisCelular,
                        Correo = p.Correo,
                        AreaTrabajo = p.AreaTrabajo,
                        Industria = p.Industria,
                        Desuscrito = p.Desuscrito,
                        Estado = p.Estado
                    })
                    .ToList();

                respuesta.Persona = lista;
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

        public VTAModVentaTPersonaDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTPersonaDTO();
            try
            {
                var ent = _unitOfWork.PersonaRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdPais = ent.IdPais;
                    dto.Nombres = ent.Nombres;
                    dto.Apellidos = ent.Apellidos;
                    dto.Celular = ent.Celular;
                    dto.PrefijoPaisCelular = ent.PrefijoPaisCelular;
                    dto.Correo = ent.Correo;
                    dto.AreaTrabajo = ent.AreaTrabajo;
                    dto.Industria = ent.Industria;
                    dto.Desuscrito = ent.Desuscrito;
                    dto.Estado = ent.Estado;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTPersonaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new Persona
                {
                    IdPais = dto.IdPais,
                    Nombres = dto.Nombres,
                    Apellidos = dto.Apellidos,
                    Celular = dto.Celular,
                    PrefijoPaisCelular = dto.PrefijoPaisCelular,
                    Correo = dto.Correo,
                    AreaTrabajo = dto.AreaTrabajo,
                    Industria = dto.Industria,
                    Desuscrito = dto.Desuscrito,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.PersonaRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTPersonaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.PersonaRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdPais = dto.IdPais;
                ent.Nombres = dto.Nombres;
                ent.Apellidos = dto.Apellidos;
                ent.Celular = dto.Celular;
                ent.PrefijoPaisCelular = dto.PrefijoPaisCelular;
                ent.Correo = dto.Correo;
                ent.AreaTrabajo = dto.AreaTrabajo;
                ent.Industria = dto.Industria;
                ent.Desuscrito = dto.Desuscrito;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.PersonaRepository.Actualizar(ent);
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
                var success = _unitOfWork.PersonaRepository.Eliminar(id);
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
