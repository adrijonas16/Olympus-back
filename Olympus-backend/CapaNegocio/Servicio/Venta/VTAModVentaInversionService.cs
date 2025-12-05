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
    public class VTAModVentaInversionService : IVTAModVentaInversionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaInversionService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaInversionDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaInversionDTORPT();
            try
            {
                var lista = _unitOfWork.InversionRepository
                    .Query()
                    .AsNoTracking()
                    .Include(i => i.Producto)
                    .Include(i => i.Oportunidad)
                    .Select(i => new VTAModVentaInversionDTO
                    {
                        Id = i.Id,
                        IdProducto = i.IdProducto,
                        IdOportunidad = i.IdOportunidad,
                        CostoTotal = i.CostoTotal,
                        Moneda = i.Moneda ?? string.Empty,
                        DescuentoPorcentaje = i.DescuentoPorcentaje,
                        CostoOfrecido = i.CostoOfrecido,
                        Estado = i.Estado,
                        FechaCreacion = i.FechaCreacion,
                        UsuarioCreacion = i.UsuarioCreacion,
                        FechaModificacion = i.FechaModificacion,
                        UsuarioModificacion = i.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Inversiones = lista;
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

        public VTAModVentaInversionDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaInversionDTO();
            try
            {
                var ent = _unitOfWork.InversionRepository
                    .Query()
                    .AsNoTracking()
                    .Include(i => i.Producto)
                    .Include(i => i.Oportunidad)
                    .Where(i => i.Id == id)
                    .Select(i => new VTAModVentaInversionDTO
                    {
                        Id = i.Id,
                        IdProducto = i.IdProducto,
                        IdOportunidad = i.IdOportunidad,
                        CostoTotal = i.CostoTotal,
                        Moneda = i.Moneda ?? string.Empty,
                        DescuentoPorcentaje = i.DescuentoPorcentaje,
                        CostoOfrecido = i.CostoOfrecido,
                        Estado = i.Estado,
                        FechaCreacion = i.FechaCreacion,
                        UsuarioCreacion = i.UsuarioCreacion,
                        FechaModificacion = i.FechaModificacion,
                        UsuarioModificacion = i.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaInversionDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validaciones: Producto y Oportunidad deben existir
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                var oportunidad = _unitOfWork.OportunidadRepository.ObtenerPorId(dto.IdOportunidad);
                if (oportunidad == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Oportunidad no encontrada.";
                    return respuesta;
                }

                // Verificar unicidad
                var existe = _unitOfWork.InversionRepository
                    .Query()
                    .AsNoTracking()
                    .Any(x => x.IdProducto == dto.IdProducto && x.IdOportunidad == dto.IdOportunidad);
                if (existe)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Ya existe una Inversión para ese Producto y Oportunidad.";
                    return respuesta;
                }

                var ent = new Inversion
                {
                    IdProducto = dto.IdProducto,
                    IdOportunidad = dto.IdOportunidad,
                    CostoTotal = dto.CostoTotal,
                    Moneda = dto.Moneda,
                    DescuentoPorcentaje = dto.DescuentoPorcentaje,
                    CostoOfrecido = dto.CostoOfrecido,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.InversionRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaInversionDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.InversionRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar existencia de Producto y Oportunidad
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                var oportunidad = _unitOfWork.OportunidadRepository.ObtenerPorId(dto.IdOportunidad);
                if (oportunidad == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Oportunidad no encontrada.";
                    return respuesta;
                }

                if (ent.IdProducto != dto.IdProducto || ent.IdOportunidad != dto.IdOportunidad)
                {
                    var existe = _unitOfWork.InversionRepository
                        .Query()
                        .AsNoTracking()
                        .Any(x => x.IdProducto == dto.IdProducto && x.IdOportunidad == dto.IdOportunidad && x.Id != dto.Id);
                    if (existe)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Otra Inversión ya existe para ese Producto y Oportunidad.";
                        return respuesta;
                    }
                }

                ent.IdProducto = dto.IdProducto;
                ent.IdOportunidad = dto.IdOportunidad;
                ent.CostoTotal = dto.CostoTotal;
                ent.Moneda = dto.Moneda;
                ent.DescuentoPorcentaje = dto.DescuentoPorcentaje;
                ent.CostoOfrecido = dto.CostoOfrecido;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.InversionRepository.Actualizar(ent);
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
                var success = _unitOfWork.InversionRepository.Eliminar(id);
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
