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
    public class VTAModVentaOcurrenciaService : IVTAModVentaOcurrenciaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaOcurrenciaService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaOcurrenciaDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaOcurrenciaDTORPT();
            try
            {
                // Traer todos los estados en memoria para evitar N+1
                var estadosDict = _unitOfWork.EstadoRepository.ObtenerTodos()
                    .ToDictionary(e => e.Id, e => e.Nombre ?? string.Empty);

                var lista = _unitOfWork.OcurrenciaRepository.ObtenerTodos()
                    .Select(c => new VTAModVentaOcurrenciaDTO
                    {
                        Id = c.Id,
                        Nombre = c.Nombre ?? string.Empty,
                        Descripcion = c.Descripcion ?? string.Empty,
                        IdEstado = c.IdEstado,
                        EstadoNombre = (c.IdEstado.HasValue && estadosDict.ContainsKey(c.IdEstado.Value)) ? estadosDict[c.IdEstado.Value] : string.Empty,
                        Estado = c.Estado,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioCreacion = c.UsuarioCreacion
                    })
                    .ToList();

                respuesta.Ocurrencias = lista;
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

        public VTAModVentaOcurrenciaDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaOcurrenciaDTO();
            try
            {
                var ent = _unitOfWork.OcurrenciaRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    // obtener nombre de estado si existe
                    string estadoNombre = string.Empty;
                    if (ent.IdEstado.HasValue)
                    {
                        var est = _unitOfWork.EstadoRepository.ObtenerPorId(ent.IdEstado.Value);
                        estadoNombre = est != null ? (est.Nombre ?? string.Empty) : string.Empty;
                    }

                    dto.Id = ent.Id;
                    dto.Nombre = ent.Nombre ?? string.Empty;
                    dto.Descripcion = ent.Descripcion ?? string.Empty;
                    dto.IdEstado = ent.IdEstado;
                    dto.EstadoNombre = estadoNombre;
                    dto.Estado = ent.Estado;
                    dto.FechaCreacion = ent.FechaCreacion;
                    dto.UsuarioCreacion = ent.UsuarioCreacion;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaOcurrenciaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Si se especificó IdEstado, validar que exista
                if (dto.IdEstado.HasValue)
                {
                    var estado = _unitOfWork.EstadoRepository.ObtenerPorId(dto.IdEstado.Value);
                    if (estado == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Estado referenciado no existe.";
                        return respuesta;
                    }
                }

                var ent = new Ocurrencia
                {
                    Nombre = dto.Nombre,
                    Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion,
                    IdEstado = dto.IdEstado,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion
                };

                _unitOfWork.OcurrenciaRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaOcurrenciaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.OcurrenciaRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Si se especificó IdEstado, validar que exista
                if (dto.IdEstado.HasValue)
                {
                    var estado = _unitOfWork.EstadoRepository.ObtenerPorId(dto.IdEstado.Value);
                    if (estado == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Estado referenciado no existe.";
                        return respuesta;
                    }
                }

                ent.Nombre = dto.Nombre;
                ent.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? null : dto.Descripcion;
                ent.IdEstado = dto.IdEstado;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.OcurrenciaRepository.Actualizar(ent);
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
                var success = _unitOfWork.OcurrenciaRepository.Eliminar(id);
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
