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
    public class VTAModVentaConvertidoService : IVTAModVentaConvertidoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaConvertidoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaConvertidoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaConvertidoDTORPT();
            try
            {
                var lista = _unitOfWork.ConvertidoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.HistorialEstado)
                    .Include(c => c.Inversion)
                    .Include(c => c.Producto)
                    .Select(c => new VTAModVentaConvertidoDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdInversion = c.IdInversion,
                        IdProducto = c.IdProducto,
                        PagoCompleto = c.PagoCompleto,
                        MontoPagado = c.MontoPagado,
                        FechaPago = c.FechaPago,
                        Moneda = c.Moneda,
                        Estado = c.Estado,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioCreacion = c.UsuarioCreacion,
                        FechaModificacion = c.FechaModificacion,
                        UsuarioModificacion = c.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Convertidos = lista;
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

        public VTAModVentaConvertidoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaConvertidoDTO();
            try
            {
                var ent = _unitOfWork.ConvertidoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.HistorialEstado)
                    .Include(c => c.Inversion)
                    .Include(c => c.Producto)
                    .Where(c => c.Id == id)
                    .Select(c => new VTAModVentaConvertidoDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdInversion = c.IdInversion,
                        IdProducto = c.IdProducto,
                        PagoCompleto = c.PagoCompleto,
                        MontoPagado = c.MontoPagado,
                        FechaPago = c.FechaPago,
                        Moneda = c.Moneda,
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaConvertidoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validaciones de Ids  
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

                var ent = new Convertido
                {
                    IdHistorialEstado = dto.IdHistorialEstado,
                    IdInversion = dto.IdInversion,
                    IdProducto = dto.IdProducto,
                    PagoCompleto = dto.PagoCompleto,
                    MontoPagado = dto.MontoPagado,
                    FechaPago = dto.FechaPago,
                    Moneda = dto.Moneda,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.ConvertidoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaConvertidoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.ConvertidoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validaciones de Ids
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
                ent.PagoCompleto = dto.PagoCompleto;
                ent.MontoPagado = dto.MontoPagado;
                ent.FechaPago = dto.FechaPago;
                ent.Moneda = dto.Moneda;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.ConvertidoRepository.Actualizar(ent);
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
                var success = _unitOfWork.ConvertidoRepository.Eliminar(id);
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
