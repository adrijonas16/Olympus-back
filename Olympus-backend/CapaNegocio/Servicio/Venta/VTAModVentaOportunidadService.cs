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
    public class VTAModVentaOportunidadService : IVTAModVentaOportunidadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaOportunidadService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTOportunidadDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTOportunidadDTORPT();
            try
            {
                var lista = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Include(o => o.Persona)
                    .Include(o => o.Producto)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        PersonaNombre = o.Persona != null ? ((o.Persona.Nombres ?? string.Empty) + " " + (o.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion
                    })
                    .ToList();

                respuesta.Oportunidad = lista;
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

        public VTAModVentaTOportunidadDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTOportunidadDTO();
            try
            {
                var ent = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Include(o => o.Persona)
                    .Include(o => o.Producto)
                    .Where(o => o.Id == id)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPersona = o.IdPersona,
                        PersonaNombre = o.Persona != null ? ((o.Persona.Nombres ?? string.Empty) + " " + (o.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                // Validar Persona (obligatoria)
                var persona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

                // Validar Producto si viene IdProducto
                if (dto.IdProducto.HasValue)
                {
                    var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (producto == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                var ent = new Oportunidad
                {
                    IdPersona = dto.IdPersona,
                    IdProducto = dto.IdProducto,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.OportunidadRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.OportunidadRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Validar Persona (obligatoria)
                var persona = _unitOfWork.PersonaRepository.ObtenerPorId(dto.IdPersona);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

                // Validar Producto si viene IdProducto
                if (dto.IdProducto.HasValue)
                {
                    var producto = _unitOfWork.ProductoRepository.ObtenerPorId(dto.IdProducto.Value);
                    if (producto == null)
                    {
                        respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                        respuesta.Mensaje = "Producto no encontrado.";
                        return respuesta;
                    }
                }

                ent.IdPersona = dto.IdPersona;
                ent.IdProducto = dto.IdProducto;
                ent.CodigoLanzamiento = dto.CodigoLanzamiento;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;

                _unitOfWork.OportunidadRepository.Actualizar(ent);
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
                var success = _unitOfWork.OportunidadRepository.Eliminar(id);
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
