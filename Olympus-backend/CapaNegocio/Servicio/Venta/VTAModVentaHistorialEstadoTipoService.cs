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
    public class VTAModVentaHistorialEstadoTipoService : IVTAModVentaHistorialEstadoTipoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHistorialEstadoTipoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaHistorialEstadoTipoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaHistorialEstadoTipoDTORPT();
            try
            {
                var lista = _unitOfWork.HistorialEstadoTipoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(h => h.Tipo)
                    .Include(h => h.HistorialEstado)
                    .Select(h => new VTAModVentaHistorialEstadoTipoDTO
                    {
                        Id = h.Id,
                        IdHistorialEstado = h.IdHistorialEstado,
                        IdTipo = h.IdTipo,
                        TipoNombre = h.Tipo != null ? h.Tipo.Nombre : string.Empty,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
                    })
                    .ToList();

                respuesta.HistorialEstadoTipos = lista;
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

        public VTAModVentaHistorialEstadoTipoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaHistorialEstadoTipoDTO();
            try
            {
                var ent = _unitOfWork.HistorialEstadoTipoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(h => h.Tipo)
                    .Include(h => h.HistorialEstado)
                    .Where(h => h.Id == id)
                    .Select(h => new VTAModVentaHistorialEstadoTipoDTO
                    {
                        Id = h.Id,
                        IdHistorialEstado = h.IdHistorialEstado,
                        IdTipo = h.IdTipo,
                        TipoNombre = h.Tipo != null ? h.Tipo.Nombre : string.Empty,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaHistorialEstadoTipoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validar existencia de HistorialEstado
                var historial = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(dto.IdHistorialEstado);
                if (historial == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "HistorialEstado no encontrado.";
                    return respuesta;
                }

                // Validar existencia de Tipo
                var tipo = _unitOfWork.TipoRepository.ObtenerPorId(dto.IdTipo);
                if (tipo == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Tipo no encontrado.";
                    return respuesta;
                }

                var ent = new HistorialEstadoTipo
                {
                    IdHistorialEstado = dto.IdHistorialEstado,
                    IdTipo = dto.IdTipo,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.HistorialEstadoTipoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaHistorialEstadoTipoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.HistorialEstadoTipoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar relaciones
                var historial = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(dto.IdHistorialEstado);
                if (historial == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "HistorialEstado no encontrado.";
                    return respuesta;
                }

                var tipo = _unitOfWork.TipoRepository.ObtenerPorId(dto.IdTipo);
                if (tipo == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Tipo no encontrado.";
                    return respuesta;
                }

                ent.IdHistorialEstado = dto.IdHistorialEstado;
                ent.IdTipo = dto.IdTipo;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.HistorialEstadoTipoRepository.Actualizar(ent);
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
                var success = _unitOfWork.HistorialEstadoTipoRepository.Eliminar(id);
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
