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
    public class VTAModVentaInversionDescuentoService : IVTAModVentaInversionDescuentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaInversionDescuentoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaInversionDescuentoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaInversionDescuentoDTORPT();
            try
            {
                var lista = _unitOfWork.InversionDescuentoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(d => d.Tipo)
                    .Include(d => d.Inversion)
                    .Select(d => new VTAModVentaInversionDescuentoDTO
                    {
                        Id = d.Id,
                        IdInversion = d.IdInversion,
                        Nombre = d.Nombre,
                        IdTipo = d.IdTipo,
                        TipoNombre = d.Tipo != null ? d.Tipo.Nombre : string.Empty,
                        Porcentaje = d.Porcentaje,
                        Monto = d.Monto,
                        FechaInicio = d.FechaInicio,
                        FechaFin = d.FechaFin,
                        Activo = d.Activo,
                        Estado = d.Estado,
                        FechaCreacion = d.FechaCreacion,
                        UsuarioCreacion = d.UsuarioCreacion,
                        FechaModificacion = d.FechaModificacion,
                        UsuarioModificacion = d.UsuarioModificacion
                    })
                    .ToList();

                respuesta.InversionDescuentos = lista;
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

        public VTAModVentaInversionDescuentoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaInversionDescuentoDTO();
            try
            {
                var ent = _unitOfWork.InversionDescuentoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(d => d.Tipo)
                    .Include(d => d.Inversion)
                    .Where(d => d.Id == id)
                    .Select(d => new VTAModVentaInversionDescuentoDTO
                    {
                        Id = d.Id,
                        IdInversion = d.IdInversion,
                        Nombre = d.Nombre,
                        IdTipo = d.IdTipo,
                        TipoNombre = d.Tipo != null ? d.Tipo.Nombre : string.Empty,
                        Porcentaje = d.Porcentaje,
                        Monto = d.Monto,
                        FechaInicio = d.FechaInicio,
                        FechaFin = d.FechaFin,
                        Activo = d.Activo,
                        Estado = d.Estado,
                        FechaCreacion = d.FechaCreacion,
                        UsuarioCreacion = d.UsuarioCreacion,
                        FechaModificacion = d.FechaModificacion,
                        UsuarioModificacion = d.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaInversionDescuentoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validaciones: Inversion obligatoria
                var inv = _unitOfWork.InversionRepository.ObtenerPorId(dto.IdInversion);
                if (inv == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Inversión no encontrada.";
                    return respuesta;
                }

                if (dto.IdTipo.HasValue)
                {
                    var tipo = _unitOfWork.TipoRepository.ObtenerPorId(dto.IdTipo.Value);
                    if (tipo == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Tipo no encontrado.";
                        return respuesta;
                    }
                }

                var ent = new InversionDescuento
                {
                    IdInversion = dto.IdInversion,
                    Nombre = dto.Nombre,
                    IdTipo = dto.IdTipo,
                    Porcentaje = dto.Porcentaje,
                    Monto = dto.Monto,
                    FechaInicio = dto.FechaInicio,
                    FechaFin = dto.FechaFin,
                    Activo = dto.Activo,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.InversionDescuentoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaInversionDescuentoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.InversionDescuentoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar Inversion
                var inv = _unitOfWork.InversionRepository.ObtenerPorId(dto.IdInversion);
                if (inv == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Inversión no encontrada.";
                    return respuesta;
                }

                // Validar Tipo si llega
                if (dto.IdTipo.HasValue)
                {
                    var tipo = _unitOfWork.TipoRepository.ObtenerPorId(dto.IdTipo.Value);
                    if (tipo == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Tipo no encontrado.";
                        return respuesta;
                    }
                }

                ent.IdInversion = dto.IdInversion;
                ent.Nombre = dto.Nombre;
                ent.IdTipo = dto.IdTipo;
                ent.Porcentaje = dto.Porcentaje;
                ent.Monto = dto.Monto;
                ent.FechaInicio = dto.FechaInicio;
                ent.FechaFin = dto.FechaFin;
                ent.Activo = dto.Activo;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.InversionDescuentoRepository.Actualizar(ent);
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
                var success = _unitOfWork.InversionDescuentoRepository.Eliminar(id);
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
