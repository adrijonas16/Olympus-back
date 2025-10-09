using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaControlOportunidadService : IVTAModVentaControlOportunidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaControlOportunidadService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTControlOportunidadDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTControlOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.ControlOportunidadRepository.ObtenerTodos()
                    .Select(c => new VTAModVentaTControlOportunidadDTO
                    {
                        Id = c.Id,
                        IdOportunidad = c.IdOportunidad,
                        Nombre = c.Nombre,
                        Url = c.Url,
                        Detalle = c.Detalle,
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

        public VTAModVentaTControlOportunidadDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTControlOportunidadDTO();
            try
            {
                var ent = _unitOfWork.ControlOportunidadRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdOportunidad = ent.IdOportunidad;
                    dto.Nombre = ent.Nombre;
                    dto.Url = ent.Url;
                    dto.Detalle = ent.Detalle;
                    dto.IdMigracion = ent.IdMigracion;
                    dto.Estado = ent.Estado;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTControlOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new ControlOportunidad
                {
                    IdOportunidad = dto.IdOportunidad,
                    Nombre = dto.Nombre,
                    Url = dto.Url,
                    Detalle = dto.Detalle,
                    IdMigracion = dto.IdMigracion,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.ControlOportunidadRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTControlOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.ControlOportunidadRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdOportunidad = dto.IdOportunidad;
                ent.Nombre = dto.Nombre;
                ent.Url = dto.Url;
                ent.Detalle = dto.Detalle;
                ent.IdMigracion = dto.IdMigracion;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.ControlOportunidadRepository.Actualizar(ent);
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
                var success = _unitOfWork.ControlOportunidadRepository.Eliminar(id);
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
