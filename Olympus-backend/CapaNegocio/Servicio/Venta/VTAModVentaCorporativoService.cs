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
    public class VTAModVentaCorporativoService : IVTAModVentaCorporativoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaCorporativoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaCorporativoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaCorporativoDTORPT();
            try
            {
                var lista = _unitOfWork.CorporativoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.Producto)
                    .Include(c => c.HistorialEstado)
                    .Select(c => new VTAModVentaCorporativoDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdProducto = c.IdProducto,
                        ProductoNombre = c.Producto != null ? c.Producto.Nombre : string.Empty,
                        IdFase = c.IdFase,
                        IdEmpresa = c.IdEmpresa,
                        Cantidad = c.Cantidad,
                        Estado = c.Estado,
                        FechaCreacion = c.FechaCreacion,
                        UsuarioCreacion = c.UsuarioCreacion,
                        FechaModificacion = c.FechaModificacion,
                        UsuarioModificacion = c.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Corporativos = lista;
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

        public VTAModVentaCorporativoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaCorporativoDTO();
            try
            {
                var ent = _unitOfWork.CorporativoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(c => c.Producto)
                    .Include(c => c.HistorialEstado)
                    .Where(c => c.Id == id)
                    .Select(c => new VTAModVentaCorporativoDTO
                    {
                        Id = c.Id,
                        IdHistorialEstado = c.IdHistorialEstado,
                        IdProducto = c.IdProducto,
                        ProductoNombre = c.Producto != null ? c.Producto.Nombre : string.Empty,
                        IdFase = c.IdFase,
                        IdEmpresa = c.IdEmpresa,
                        Cantidad = c.Cantidad,
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaCorporativoDTO dto)
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

                var ent = new Corporativo
                {
                    IdHistorialEstado = dto.IdHistorialEstado,
                    IdProducto = dto.IdProducto,
                    IdFase = dto.IdFase,
                    IdEmpresa = dto.IdEmpresa,
                    Cantidad = dto.Cantidad,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.CorporativoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaCorporativoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.CorporativoRepository.ObtenerPorId(dto.Id);
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
                ent.IdProducto = dto.IdProducto;
                ent.IdFase = dto.IdFase;
                ent.IdEmpresa = dto.IdEmpresa;
                ent.Cantidad = dto.Cantidad;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.CorporativoRepository.Actualizar(ent);
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
                var success = _unitOfWork.CorporativoRepository.Eliminar(id);
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
