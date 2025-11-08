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
    public class VTAModVentaVentaCruzadaService : IVTAModVentaVentaCruzadaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaVentaCruzadaService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaVentaCruzadaDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaVentaCruzadaDTORPT();
            try
            {
                var lista = _unitOfWork.VentaCruzadaRepository
                    .Query() // IQueryable<VentaCruzada>
                    .AsNoTracking()
                    .Include(v => v.ProductoOrigen)
                    .Include(v => v.ProductoDestino)
                    .Include(v => v.HistorialEstado)
                    .Select(v => new VTAModVentaVentaCruzadaDTO
                    {
                        Id = v.Id,
                        IdHistorialEstado = v.IdHistorialEstado,
                        IdProductoOrigen = v.IdProductoOrigen,
                        ProductoOrigenNombre = v.ProductoOrigen != null ? v.ProductoOrigen.Nombre : string.Empty,
                        IdProductoDestino = v.IdProductoDestino,
                        ProductoDestinoNombre = v.ProductoDestino != null ? v.ProductoDestino.Nombre : string.Empty,
                        IdFase = v.IdFase,
                        Estado = v.Estado,
                        FechaCreacion = v.FechaCreacion,
                        UsuarioCreacion = v.UsuarioCreacion,
                        FechaModificacion = v.FechaModificacion,
                        UsuarioModificacion = v.UsuarioModificacion
                    })
                    .ToList();

                respuesta.VentaCruzadas = lista;
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

        public VTAModVentaVentaCruzadaDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaVentaCruzadaDTO();
            try
            {
                var ent = _unitOfWork.VentaCruzadaRepository
                    .Query()
                    .AsNoTracking()
                    .Include(v => v.ProductoOrigen)
                    .Include(v => v.ProductoDestino)
                    .Include(v => v.HistorialEstado)
                    .Where(v => v.Id == id)
                    .Select(v => new VTAModVentaVentaCruzadaDTO
                    {
                        Id = v.Id,
                        IdHistorialEstado = v.IdHistorialEstado,
                        IdProductoOrigen = v.IdProductoOrigen,
                        ProductoOrigenNombre = v.ProductoOrigen != null ? v.ProductoOrigen.Nombre : string.Empty,
                        IdProductoDestino = v.IdProductoDestino,
                        ProductoDestinoNombre = v.ProductoDestino != null ? v.ProductoDestino.Nombre : string.Empty,
                        IdFase = v.IdFase,
                        Estado = v.Estado,
                        FechaCreacion = v.FechaCreacion,
                        UsuarioCreacion = v.UsuarioCreacion,
                        FechaModificacion = v.FechaModificacion,
                        UsuarioModificacion = v.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaVentaCruzadaDTO dto)
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

                if (dto.IdProductoOrigen.HasValue)
                {
                    var po = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProductoOrigen.Value);
                    if (po == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto origen no encontrado.";
                        return respuesta;
                    }
                }

                if (dto.IdProductoDestino.HasValue)
                {
                    var pd = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProductoDestino.Value);
                    if (pd == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto destino no encontrado.";
                        return respuesta;
                    }
                }

                var ent = new VentaCruzada
                {
                    IdHistorialEstado = dto.IdHistorialEstado,
                    IdProductoOrigen = dto.IdProductoOrigen,
                    IdProductoDestino = dto.IdProductoDestino,
                    IdFase = dto.IdFase,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.VentaCruzadaRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaVentaCruzadaDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.VentaCruzadaRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validaciones si se envían Ids
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

                if (dto.IdProductoOrigen.HasValue)
                {
                    var po = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProductoOrigen.Value);
                    if (po == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto origen no encontrado.";
                        return respuesta;
                    }
                }

                if (dto.IdProductoDestino.HasValue)
                {
                    var pd = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProductoDestino.Value);
                    if (pd == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto destino no encontrado.";
                        return respuesta;
                    }
                }

                ent.IdHistorialEstado = dto.IdHistorialEstado;
                ent.IdProductoOrigen = dto.IdProductoOrigen;
                ent.IdProductoDestino = dto.IdProductoDestino;
                ent.IdFase = dto.IdFase;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.VentaCruzadaRepository.Actualizar(ent);
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
                var success = _unitOfWork.VentaCruzadaRepository.Eliminar(id);
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
