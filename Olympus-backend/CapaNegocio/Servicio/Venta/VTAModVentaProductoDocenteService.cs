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
    public class VTAModVentaProductoDocenteService : IVTAModVentaProductoDocenteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaProductoDocenteService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaProductoDocenteDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaProductoDocenteDTORPT();
            try
            {
                var lista = _unitOfWork.ProductoDocenteRepository
                    .Query() // IQueryable<ProductoDocente>
                    .AsNoTracking()
                    .Include(pd => pd.Producto)
                    .Include(pd => pd.Docente)
                    .Select(pd => new VTAModVentaProductoDocenteDTO
                    {
                        Id = pd.Id,
                        IdProducto = pd.IdProducto,
                        IdDocente = pd.IdDocente,
                        Orden = pd.Orden,
                        Estado = pd.Estado,
                        FechaCreacion = pd.FechaCreacion,
                        UsuarioCreacion = pd.UsuarioCreacion
                    })
                    .ToList();

                respuesta.ProductoDocentes = lista;
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

        public VTAModVentaProductoDocenteDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaProductoDocenteDTO();
            try
            {
                var ent = _unitOfWork.ProductoDocenteRepository
                    .Query()
                    .AsNoTracking()
                    .Include(pd => pd.Producto)
                    .Include(pd => pd.Docente)
                    .Where(pd => pd.Id == id)
                    .Select(pd => new VTAModVentaProductoDocenteDTO
                    {
                        Id = pd.Id,
                        IdProducto = pd.IdProducto,
                        IdDocente = pd.IdDocente,
                        Orden = pd.Orden,
                        Estado = pd.Estado,
                        FechaCreacion = pd.FechaCreacion,
                        UsuarioCreacion = pd.UsuarioCreacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoDocenteDTO dto)
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

                // Validar existencia de Docente
                var docente = _unitOfWork.DocenteRepository.ObtenerPorId(dto.IdDocente);
                if (docente == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Docente no encontrado.";
                    return respuesta;
                }

                var ent = new ProductoDocente
                {
                    IdProducto = dto.IdProducto,
                    IdDocente = dto.IdDocente,
                    Orden = dto.Orden,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion
                };

                _unitOfWork.ProductoDocenteRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoDocenteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.ProductoDocenteRepository.ObtenerPorId(dto.Id);
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

                // Validar existencia de Docente
                var docente = _unitOfWork.DocenteRepository.ObtenerPorId(dto.IdDocente);
                if (docente == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Docente no encontrado.";
                    return respuesta;
                }

                ent.IdProducto = dto.IdProducto;
                ent.IdDocente = dto.IdDocente;
                ent.Orden = dto.Orden;
                ent.Estado = dto.Estado;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;
                ent.FechaModificacion = DateTime.UtcNow;

                _unitOfWork.ProductoDocenteRepository.Actualizar(ent);
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
                var success = _unitOfWork.ProductoDocenteRepository.Eliminar(id);
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
