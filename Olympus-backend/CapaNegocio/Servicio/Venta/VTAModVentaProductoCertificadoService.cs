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
    public class VTAModVentaProductoCertificadoService : IVTAModVentaProductoCertificadoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaProductoCertificadoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaProductoCertificadoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaProductoCertificadoDTORPT();
            try
            {
                var lista = _unitOfWork.ProductoCertificadoRepository
                    .Query() // IQueryable<ProductoCertificado>
                    .AsNoTracking()
                    .Include(pc => pc.Producto)
                    .Include(pc => pc.Certificado)
                    .Select(pc => new VTAModVentaProductoCertificadoDTO
                    {
                        Id = pc.Id,
                        IdProducto = pc.IdProducto,
                        IdCertificado = pc.IdCertificado,
                        Estado = pc.Estado,
                        FechaCreacion = pc.FechaCreacion,
                        UsuarioCreacion = pc.UsuarioCreacion,
                        FechaModificacion = pc.FechaModificacion,
                        UsuarioModificacion = pc.UsuarioModificacion
                    })
                    .ToList();

                respuesta.ProductoCertificados = lista;
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

        public VTAModVentaProductoCertificadoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaProductoCertificadoDTO();
            try
            {
                var ent = _unitOfWork.ProductoCertificadoRepository
                    .Query()
                    .AsNoTracking()
                    .Include(pc => pc.Producto)
                    .Include(pc => pc.Certificado)
                    .Where(pc => pc.Id == id)
                    .Select(pc => new VTAModVentaProductoCertificadoDTO
                    {
                        Id = pc.Id,
                        IdProducto = pc.IdProducto,
                        IdCertificado = pc.IdCertificado,
                        Estado = pc.Estado,
                        FechaCreacion = pc.FechaCreacion,
                        UsuarioCreacion = pc.UsuarioCreacion,
                        FechaModificacion = pc.FechaModificacion,
                        UsuarioModificacion = pc.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoCertificadoDTO dto)
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

                // Validar existencia de Certificado
                var certificado = _unitOfWork.CertificadoRepository.ObtenerPorId(dto.IdCertificado);
                if (certificado == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Certificado no encontrado.";
                    return respuesta;
                }

                var ent = new ProductoCertificado
                {
                    IdProducto = dto.IdProducto,
                    IdCertificado = dto.IdCertificado,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = dto.FechaModificacion == default ? DateTime.UtcNow : dto.FechaModificacion,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.ProductoCertificadoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoCertificadoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.ProductoCertificadoRepository.ObtenerPorId(dto.Id);
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

                // Validar existencia de Certificado
                var certificado = _unitOfWork.CertificadoRepository.ObtenerPorId(dto.IdCertificado);
                if (certificado == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Certificado no encontrado.";
                    return respuesta;
                }

                ent.IdProducto = dto.IdProducto;
                ent.IdCertificado = dto.IdCertificado;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.ProductoCertificadoRepository.Actualizar(ent);
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
                var success = _unitOfWork.ProductoCertificadoRepository.Eliminar(id);
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
