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
    public class VTAModVentaBeneficioService : IVTAModVentaBeneficioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaBeneficioService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaBeneficioDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaBeneficioDTORPT();
            try
            {
                var lista = _unitOfWork.BeneficioRepository
                    .Query() // IQueryable<Beneficio>
                    .AsNoTracking()
                    .Include(b => b.Producto)
                    .Select(b => new VTAModVentaBeneficioDTO
                    {
                        Id = b.Id,
                        IdProducto = b.IdProducto,
                        Descripcion = b.Descripcion ?? string.Empty,
                        Orden = b.Orden,
                        Estado = b.Estado,
                        FechaCreacion = b.FechaCreacion,
                        UsuarioCreacion = b.UsuarioCreacion,
                        FechaModificacion = b.FechaModificacion,
                        UsuarioModificacion = b.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Beneficios = lista;
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

        public VTAModVentaBeneficioDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaBeneficioDTO();
            try
            {
                var ent = _unitOfWork.BeneficioRepository
                    .Query()
                    .AsNoTracking()
                    .Include(b => b.Producto)
                    .Where(b => b.Id == id)
                    .Select(b => new VTAModVentaBeneficioDTO
                    {
                        Id = b.Id,
                        IdProducto = b.IdProducto,
                        Descripcion = b.Descripcion ?? string.Empty,
                        Orden = b.Orden,
                        Estado = b.Estado,
                        FechaCreacion = b.FechaCreacion,
                        UsuarioCreacion = b.UsuarioCreacion,
                        FechaModificacion = b.FechaModificacion,
                        UsuarioModificacion = b.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaBeneficioDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // validar existencia de producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                var ent = new Beneficio
                {
                    IdProducto = dto.IdProducto,
                    Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? string.Empty : dto.Descripcion,
                    Orden = dto.Orden,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.BeneficioRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaBeneficioDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.BeneficioRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // validar existencia de producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                ent.IdProducto = dto.IdProducto;
                ent.Descripcion = string.IsNullOrWhiteSpace(dto.Descripcion) ? string.Empty : dto.Descripcion;
                ent.Orden = dto.Orden;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.BeneficioRepository.Actualizar(ent);
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
                var success = _unitOfWork.BeneficioRepository.Eliminar(id);
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
