using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaEstadoService : IVTAModVentaEstadoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaEstadoService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTEstadoDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTEstadoDTORPT();
            try
            {
                var estados = _unitOfWork.EstadoRepository.ObtenerTodos().ToList();

                var tipoIds = estados.Where(e => e.IdTipo > 0).Select(e => e.IdTipo).Distinct().ToList();
                var tipos = _unitOfWork.TipoRepository.ObtenerTodos()
                    .Where(t => tipoIds.Contains(t.Id))
                    .ToDictionary(t => t.Id, t => t.Nombre);

                var lista = estados.Select(e => new VTAModVentaTEstadoDTO
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    Descripcion = e.Descripcion,
                    IdTipo = e.IdTipo,
                    TipoNombre = tipos.ContainsKey(e.IdTipo) ? tipos[e.IdTipo] : string.Empty,
                    TipoCategoria = tipos.ContainsKey(e.IdTipo) ? (_unitOfWork.TipoRepository.ObtenerPorId(e.IdTipo)?.Categoria ?? string.Empty) : string.Empty,
                    Estado = e.EstadoControl,
                    FechaCreacion = e.FechaCreacion,
                    UsuarioCreacion = e.UsuarioCreacion,
                    FechaModificacion = e.FechaModificacion,
                    UsuarioModificacion = e.UsuarioModificacion
                }).ToList();

                respuesta.Estados = lista;
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

        public VTAModVentaTEstadoDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTEstadoDTO();
            try
            {
                var ent = _unitOfWork.EstadoRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.Nombre = ent.Nombre;
                    dto.Descripcion = ent.Descripcion;
                    dto.IdTipo = ent.IdTipo;
                    if (ent.IdTipo > 0)
                    {
                        var tipo = _unitOfWork.TipoRepository.ObtenerPorId(ent.IdTipo);
                        dto.TipoNombre = tipo?.Nombre ?? string.Empty;
                        dto.TipoCategoria = tipo?.Categoria ?? string.Empty;
                    }
                    dto.Estado = ent.EstadoControl;
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTEstadoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new Estado
                {
                    Nombre = dto.Nombre?.Trim() ?? string.Empty,
                    Descripcion = dto.Descripcion?.Trim() ?? string.Empty,
                    IdTipo = dto.IdTipo.HasValue ? dto.IdTipo.Value : 0,
                    EstadoControl = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion,
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion
                };

                _unitOfWork.EstadoRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTEstadoDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.EstadoRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.Nombre = dto.Nombre;
                ent.Descripcion = dto.Descripcion;
                ent.EstadoControl = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.EstadoRepository.Actualizar(ent);
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
                var success = _unitOfWork.EstadoRepository.Eliminar(id);
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
