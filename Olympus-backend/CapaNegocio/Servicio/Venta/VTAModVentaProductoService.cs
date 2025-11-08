using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaProductoService : IVTAModVentaProductoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaProductoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaProductoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaProductoDTORPT();
            try
            {
                var productos = _unitOfWork.ProductoRepository.ObtenerTodos().ToList();

                // Evitar N+1: traer lanzamientos en memoria
                var lanzamientoIds = productos.Where(p => p.IdLanzamiento > 0).Select(p => p.IdLanzamiento).Distinct().ToList();
                var lanzamientos = _unitOfWork.LanzamientoRepository.ObtenerTodos()
                    .Where(l => lanzamientoIds.Contains(l.Id))
                    .ToDictionary(l => l.Id, l => l.CodigoLanzamiento);

                var lista = productos.Select(p => new VTAModVentaProductoDTO
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    IdLanzamiento = p.IdLanzamiento,
                    LanzamientoCodigo = p.IdLanzamiento > 0 && lanzamientos.ContainsKey(p.IdLanzamiento) ? lanzamientos[p.IdLanzamiento] : string.Empty,
                    CodigoLanzamiento = p.CodigoLanzamiento,
                    FechaInicio = p.FechaInicio,
                    FechaPresentacion = p.FechaPresentacion,
                    DatosImportantes = p.DatosImportantes,
                    Estado = p.Estado,
                    FechaCreacion = p.FechaCreacion,
                    UsuarioCreacion = p.UsuarioCreacion,
                    FechaModificacion = p.FechaModificacion,
                    UsuarioModificacion = p.UsuarioModificacion
                }).ToList();

                respuesta.Productos = lista;
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

        public VTAModVentaProductoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaProductoDTO();
            try
            {
                var ent = _unitOfWork.ProductoRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.Nombre = ent.Nombre;
                    dto.IdLanzamiento = ent.IdLanzamiento;
                    dto.LanzamientoCodigo = ent.IdLanzamiento > 0 ? (_unitOfWork.LanzamientoRepository.ObtenerPorId(ent.IdLanzamiento)?.CodigoLanzamiento ?? string.Empty) : string.Empty;
                    dto.CodigoLanzamiento = ent.CodigoLanzamiento;
                    dto.FechaInicio = ent.FechaInicio;
                    dto.FechaPresentacion = ent.FechaPresentacion;
                    dto.DatosImportantes = ent.DatosImportantes;
                    dto.Estado = ent.Estado;
                    dto.FechaCreacion = ent.FechaCreacion;
                    dto.UsuarioCreacion = ent.UsuarioCreacion;
                    dto.FechaModificacion = ent.FechaModificacion;
                    dto.UsuarioModificacion = ent.UsuarioModificacion;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new Producto
                {
                    Nombre = dto.Nombre?? string.Empty,
                    IdLanzamiento = dto.IdLanzamiento,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    FechaInicio = dto.FechaInicio,
                    FechaPresentacion = dto.FechaPresentacion,
                    DatosImportantes = dto.DatosImportantes,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.ProductoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.ProductoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.Nombre = dto.Nombre ?? ent.Nombre;
                ent.IdLanzamiento = dto.IdLanzamiento;
                ent.CodigoLanzamiento = dto.CodigoLanzamiento;
                ent.FechaInicio = dto.FechaInicio;
                ent.FechaPresentacion = dto.FechaPresentacion;
                ent.DatosImportantes = dto.DatosImportantes;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.ProductoRepository.Actualizar(ent);
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
                var success = _unitOfWork.ProductoRepository.Eliminar(id);
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
