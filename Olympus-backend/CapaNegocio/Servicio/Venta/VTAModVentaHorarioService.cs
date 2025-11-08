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
    public class VTAModVentaHorarioService : IVTAModVentaHorarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaHorarioService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaHorarioDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaHorarioDTORPT();
            try
            {
                // Una sola llamada que incluye Producto y proyecta a DTO
                var lista = _unitOfWork.HorarioRepository
                    .Query()                                 // IQueryable<Horario>
                    .AsNoTracking()
                    .Include(h => h.Producto)
                    .Select(h => new VTAModVentaHorarioDTO
                    {
                        Id = h.Id,
                        IdProducto = h.IdProducto,
                        ProductoNombre = h.Producto != null ? (h.Producto.Nombre ?? string.Empty) : string.Empty,
                        Dia = h.Dia ?? string.Empty,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin,
                        Detalle = h.Detalle ?? string.Empty,
                        Orden = h.Orden,
                        Estado = h.Estado,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Horarios = lista;
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

        public VTAModVentaHorarioDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaHorarioDTO();
            try
            {
                var ent = _unitOfWork.HorarioRepository
                    .Query()
                    .AsNoTracking()
                    .Include(h => h.Producto)
                    .Where(h => h.Id == id)
                    .Select(h => new VTAModVentaHorarioDTO
                    {
                        Id = h.Id,
                        IdProducto = h.IdProducto,
                        ProductoNombre = h.Producto != null ? (h.Producto.Nombre ?? string.Empty) : string.Empty,
                        Dia = h.Dia ?? string.Empty,
                        HoraInicio = h.HoraInicio,
                        HoraFin = h.HoraFin,
                        Detalle = h.Detalle ?? string.Empty,
                        Orden = h.Orden,
                        Estado = h.Estado,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaHorarioDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validar que exista el Producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                var ent = new Horario
                {
                    IdProducto = dto.IdProducto,
                    Dia = dto.Dia,
                    HoraInicio = dto.HoraInicio,
                    HoraFin = dto.HoraFin,
                    Detalle = string.IsNullOrWhiteSpace(dto.Detalle) ? null : dto.Detalle,
                    Orden = dto.Orden,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.HorarioRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaHorarioDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.HorarioRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar Producto
                var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto);
                if (producto == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Producto no encontrado.";
                    return respuesta;
                }

                ent.IdProducto = dto.IdProducto;
                ent.Dia = dto.Dia;
                ent.HoraInicio = dto.HoraInicio;
                ent.HoraFin = dto.HoraFin;
                ent.Detalle = string.IsNullOrWhiteSpace(dto.Detalle) ? null : dto.Detalle;
                ent.Orden = dto.Orden;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.HorarioRepository.Actualizar(ent);
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
                var success = _unitOfWork.HorarioRepository.Eliminar(id);
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
