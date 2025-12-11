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
    public class VTAModVentaMetodoPagoProductoService : IVTAModVentaMetodoPagoProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaMetodoPagoProductoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaMetodoPagoProductoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaMetodoPagoProductoDTORPT();
            try
            {
                var lista = _unitOfWork.MetodoPagoProductoRepository
                    .Query() // IQueryable<MetodoPagoProducto>
                    .AsNoTracking()
                    .Include(mpp => mpp.Producto)
                    .Include(mpp => mpp.MetodoPago)
                    .Select(mpp => new VTAModVentaMetodoPagoProductoDTO
                    {
                        Id = mpp.Id,
                        IdProducto = mpp.IdProducto,
                        IdMetodoPago = mpp.IdMetodoPago,
                        Activo = mpp.Activo,
                        FechaCreacion = mpp.FechaCreacion,
                        UsuarioCreacion = mpp.UsuarioCreacion
                    })
                    .ToList();

                respuesta.MetodoPagoProductos = lista;
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

        public VTAModVentaMetodoPagoProductoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaMetodoPagoProductoDTO();
            try
            {
                var ent = _unitOfWork.MetodoPagoProductoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(mpp => mpp.Producto)
                    .Include(mpp => mpp.MetodoPago)
                    .Where(mpp => mpp.Id == id)
                    .Select(mpp => new VTAModVentaMetodoPagoProductoDTO
                    {
                        Id = mpp.Id,
                        IdProducto = mpp.IdProducto,
                        IdMetodoPago = mpp.IdMetodoPago,
                        Activo = mpp.Activo,
                        FechaCreacion = mpp.FechaCreacion,
                        UsuarioCreacion = mpp.UsuarioCreacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaMetodoPagoProductoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validar existencia de Producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                // Validar existencia de MetodoPago
                var metodo = _unitOfWork.MetodoPagoRepository.ObtenerPorId(dto.IdMetodoPago);
                if (metodo == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Método de pago no encontrado.";
                    return respuesta;
                }

                var ent = new MetodoPagoProducto
                {
                    IdProducto = dto.IdProducto,
                    IdMetodoPago = dto.IdMetodoPago,
                    Activo = dto.Activo,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion
                };

                _unitOfWork.MetodoPagoProductoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaMetodoPagoProductoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.MetodoPagoProductoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar existencia de Producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                // Validar existencia de MetodoPago
                var metodo = _unitOfWork.MetodoPagoRepository.ObtenerPorId(dto.IdMetodoPago);
                if (metodo == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Método de pago no encontrado.";
                    return respuesta;
                }

                ent.IdProducto = dto.IdProducto;
                ent.IdMetodoPago = dto.IdMetodoPago;
                ent.Activo = dto.Activo;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.MetodoPagoProductoRepository.Actualizar(ent);
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
                var success = _unitOfWork.MetodoPagoProductoRepository.Eliminar(id);
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
