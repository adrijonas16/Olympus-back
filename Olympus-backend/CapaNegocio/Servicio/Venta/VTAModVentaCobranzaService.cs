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
    public class VTAModVentaCobranzaService : IVTAModVentaCobranzaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaCobranzaService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaCobranzaDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaCobranzaDTORPT();
            try
            {
                var lista = _unitOfWork.CobranzaRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.HistorialEstado)
                    .Include(c => c.Inversion)
                    .Include(c => c.Producto)
                    .Select(c => new VTAModVentaCobranzaDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdInversion = c.IdInversion,
                        IdProducto = c.IdProducto,
                        MontoTotal = c.MontoTotal,
                        NumeroCuotas = c.NumeroCuotas,
                        MontoPorCuota = c.MontoPorCuota,
                        MontoPagado = c.MontoPagado,
                        MontoRestante = c.MontoRestante,
                        Estado = c.Estado,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioCreacion = c.UsuarioCreacion,
                        FechaModificacion = c.FechaModificacion,
                        UsuarioModificacion = c.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Cobranzas = lista;
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

        public VTAModVentaCobranzaDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaCobranzaDTO();
            try
            {
                var ent = _unitOfWork.CobranzaRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.HistorialEstado)
                    .Include(c => c.Inversion)
                    .Include(c => c.Producto)
                    .Where(c => c.Id == id)
                    .Select(c => new VTAModVentaCobranzaDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdInversion = c.IdInversion,
                        IdProducto = c.IdProducto,
                        MontoTotal = c.MontoTotal,
                        NumeroCuotas = c.NumeroCuotas,
                        MontoPorCuota = c.MontoPorCuota,
                        MontoPagado = c.MontoPagado,
                        MontoRestante = c.MontoRestante,
                        Estado = c.Estado,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioCreacion = c.UsuarioCreacion,
                        FechaModificacion = c.FechaModificacion,
                        UsuarioModificacion = c.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaCobranzaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validaciones de Ids relacionados
                if (dto.IdHistorialEstado.HasValue)
                {
                    var he = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(dto.IdHistorialEstado.Value);
                    if (he == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "HistorialEstado no encontrado.";
                        return respuesta;
                    }
                }

                if (dto.IdInversion.HasValue)
                {
                    var inv = _unitOfWork.InversionRepository.ObtenerPorId(dto.IdInversion.Value);
                    if (inv == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Inversión no encontrada.";
                        return respuesta;
                    }
                }

                if (dto.IdProducto.HasValue)
                {
                    var prod = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (prod == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                var ent = new Cobranza
                {
                    IdHistorialEstado = dto.IdHistorialEstado,
                    IdInversion = dto.IdInversion,
                    IdProducto = dto.IdProducto,
                    MontoTotal = dto.MontoTotal,
                    NumeroCuotas = dto.NumeroCuotas,
                    MontoPorCuota = dto.MontoPorCuota,
                    MontoPagado = dto.MontoPagado,
                    MontoRestante = dto.MontoRestante,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.CobranzaRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaCobranzaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.CobranzaRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validaciones opcionales si se envían Ids
                if (dto.IdHistorialEstado.HasValue)
                {
                    var he = _unitOfWork.HistorialEstadoRepository.ObtenerPorId(dto.IdHistorialEstado.Value);
                    if (he == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "HistorialEstado no encontrado.";
                        return respuesta;
                    }
                }

                if (dto.IdInversion.HasValue)
                {
                    var inv = _unitOfWork.InversionRepository.ObtenerPorId(dto.IdInversion.Value);
                    if (inv == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Inversión no encontrada.";
                        return respuesta;
                    }
                }

                if (dto.IdProducto.HasValue)
                {
                    var prod = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (prod == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                ent.IdHistorialEstado = dto.IdHistorialEstado;
                ent.IdInversion = dto.IdInversion;
                ent.IdProducto = dto.IdProducto;
                ent.MontoTotal = dto.MontoTotal;
                ent.NumeroCuotas = dto.NumeroCuotas;
                ent.MontoPorCuota = dto.MontoPorCuota;
                ent.MontoPagado = dto.MontoPagado;
                ent.MontoRestante = dto.MontoRestante;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.CobranzaRepository.Actualizar(ent);
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
                var success = _unitOfWork.CobranzaRepository.Eliminar(id);
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
