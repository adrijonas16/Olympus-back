using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

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
                    .Include(o => o.PotencialCliente)
                        .ThenInclude(pc => pc.Persona)
                    .Include(o => o.Producto)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPotencialCliente = o.IdPotencialCliente,
                        PersonaNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null 
                            ? ((o.PotencialCliente.Persona.Nombres ?? string.Empty) + " " + (o.PotencialCliente.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Origen = o.Origen,
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
                    .Include(o => o.PotencialCliente)
                        .ThenInclude(pc => pc.Persona)
                    .Include(o => o.Producto)
                    .Where(o => o.Id == id)
                    .Select(o => new VTAModVentaTOportunidadDTO
                    {
                        Id = o.Id,
                        IdPotencialCliente = o.IdPotencialCliente,
                        PersonaNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null
                            ? ((o.PotencialCliente.Persona.Nombres ?? string.Empty) + " " + (o.PotencialCliente.Persona.Apellidos ?? string.Empty)).Trim() : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Origen = o.Origen,
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
                // Validar Persona
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
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
                    IdPotencialCliente = dto.IdPotencialCliente,
                    IdProducto = dto.IdProducto,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Origen = dto.Origen,
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

                // Validar Persona
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
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

                ent.IdPotencialCliente = dto.IdPotencialCliente;
                ent.IdProducto = dto.IdProducto;
                ent.CodigoLanzamiento = dto.CodigoLanzamiento;
                ent.Origen = dto.Origen;
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

        public VTAModVentaOportunidadDetalleDTORPT ObtenerDetallePorId(int id)
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();

            try
            {
                var oportunidadQuery = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Include(o => o.PotencialCliente)
                        .ThenInclude(pc => pc.Persona)
                    .Include(o => o.Producto)
                    .Where(o => o.Id == id);

                var oportunidad = oportunidadQuery
                    .Select(o => new VTAModVentaOportunidadDetalleDTO
                    {
                        Id = o.Id,
                        IdPotencialCliente = o.IdPotencialCliente,
                        PersonaNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null
                        ? ((o.PotencialCliente.Persona.Nombres ?? string.Empty) + " " + (o.PotencialCliente.Persona.Apellidos ?? string.Empty)).Trim()
                        : string.Empty,
                        IdProducto = o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : string.Empty,
                        CodigoLanzamiento = o.CodigoLanzamiento,
                        Origen = o.Origen,
                        Estado = o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion ?? string.Empty,
                        TotalOportunidadesPersona = 0,
                    })
                    .FirstOrDefault();

                if (oportunidad == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                // Obtener el último HistorialEstado creado
                var ultimoHistorial = _unitOfWork.HistorialEstadoRepository
                    .Query()
                    .AsNoTracking()
                    .Where(h => h.IdOportunidad == id)
                    .OrderByDescending(h => h.FechaCreacion)
                    .ThenByDescending(h => h.Id)
                    .Select(h => new
                    {
                        h.Id,
                        h.IdOportunidad,
                        h.IdAsesor,
                        h.IdEstado,
                        h.Observaciones,
                        h.CantidadLlamadasContestadas,
                        h.CantidadLlamadasNoContestadas,
                        h.Estado,
                        h.FechaCreacion,
                        h.FechaModificacion,
                        h.UsuarioCreacion,
                        h.UsuarioModificacion
                    })
                    .FirstOrDefault();

                VTAModVentaTHistorialEstadoDetalleDTO? historialDto = null;

                if (ultimoHistorial != null)
                {
                    // Asesor
                    VTAModVentaTAsesorDTO? asesorDto = null;
                    if (ultimoHistorial.IdAsesor.HasValue)
                    {
                        var asesor = _unitOfWork.AsesorRepository
                            .Query()
                            .AsNoTracking()
                            .Where(a => a.Id == ultimoHistorial.IdAsesor.Value)
                            .Select(a => new { a.Id, a.Nombres, a.Apellidos, a.Correo, a.Celular })
                            .FirstOrDefault();

                        if (asesor != null)
                        {
                            asesorDto = new VTAModVentaTAsesorDTO
                            {
                                Id = asesor.Id,
                                Nombres = asesor.Nombres ?? string.Empty,
                                Apellidos = asesor.Apellidos ?? string.Empty,
                                Correo = asesor.Correo ?? string.Empty,
                                Celular = asesor.Celular ?? string.Empty
                            };
                        }
                    }

                    // Estado referencia
                    VTAModVentaTEstadoDTO? estadoRefDto = null;
                    if (ultimoHistorial.IdEstado.HasValue)
                    {
                        var estadoRef = _unitOfWork.EstadoRepository
                            .Query()
                            .AsNoTracking()
                            .Where(e => e.Id == ultimoHistorial.IdEstado.Value)
                            .Select(e => new { e.Id, e.Nombre, IdTipo = (int?)e.IdTipo })
                            .FirstOrDefault();

                        if (estadoRef != null)
                        {
                            string tipoNombre = string.Empty;
                            string tipoCategoria = string.Empty;

                            if (estadoRef.IdTipo.HasValue)
                            {
                                var tipo = _unitOfWork.TipoRepository
                                    .Query()
                                    .AsNoTracking()
                                    .Where(t => t.Id == estadoRef.IdTipo.Value)
                                    .Select(t => new { t.Id, t.Nombre, t.Categoria })
                                    .FirstOrDefault();

                                if (tipo != null)
                                {
                                    tipoNombre = tipo.Nombre ?? string.Empty;
                                    tipoCategoria = tipo.Categoria ?? string.Empty;
                                }
                            }

                            estadoRefDto = new VTAModVentaTEstadoDTO
                            {
                                Id = estadoRef.Id,
                                Nombre = estadoRef.Nombre ?? string.Empty,
                                IdTipo = estadoRef.IdTipo ?? 0,
                                TipoNombre = tipoNombre,
                                TipoCategoria = tipoCategoria
                            };
                        }
                    }

                    // DTO final
                    historialDto = new VTAModVentaTHistorialEstadoDetalleDTO
                    {
                        Id = ultimoHistorial.Id,
                        IdOportunidad = ultimoHistorial.IdOportunidad,
                        IdAsesor = ultimoHistorial.IdAsesor,
                        IdEstado = ultimoHistorial.IdEstado,
                        Observaciones = ultimoHistorial.Observaciones ?? string.Empty,
                        CantidadLlamadasContestadas = ultimoHistorial.CantidadLlamadasContestadas,
                        CantidadLlamadasNoContestadas = ultimoHistorial.CantidadLlamadasNoContestadas,
                        Estado = ultimoHistorial.Estado,
                        FechaCreacion = ultimoHistorial.FechaCreacion,
                        FechaModificacion = ultimoHistorial.FechaModificacion,
                        UsuarioCreacion = ultimoHistorial.UsuarioCreacion ?? string.Empty,
                        UsuarioModificacion = ultimoHistorial.UsuarioModificacion
                    };

                    historialDto.Asesor = asesorDto;
                    historialDto.EstadoReferencia = estadoRefDto;

                    respuesta.HistorialActual.Add(historialDto);
                }

                // Calcular total de historialEstados del mismo IdPotencialCliente donde IdAsesor es NULL
                var totalHistorialesSinAsesor = _unitOfWork.HistorialEstadoRepository
                    .Query()
                    .AsNoTracking()
                    .Where(h => h.IdAsesor == null
                                && _unitOfWork.OportunidadRepository
                                      .Query()
                                      .Any(o => o.Id == h.IdOportunidad && o.IdPotencialCliente == oportunidad.IdPotencialCliente))
                    .Count();

                // Rellenar TotalOportunidadesPersona y UltimoHistorial 
                oportunidad.TotalOportunidadesPersona = totalHistorialesSinAsesor;

                // Agregar a la respuesta
                respuesta.Oportunidad.Add(oportunidad);

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

        public VTAModVentaTHistorialInteraccionDTORPT ObtenerHistorialInteraccionesPorOportunidad(int id, int? idTipo = null)
        {
            var respuesta = new VTAModVentaTHistorialInteraccionDTORPT();

            try
            {
                var query = _unitOfWork.HistorialInteraccionRepository
                            .Query()
                            .AsNoTracking()
                            .Where(h => h.IdOportunidad == id);

                if (idTipo.HasValue)
                    query = query.Where(h => h.IdTipo == idTipo.Value);

                var lista = query
                    .OrderBy(h => h.FechaCreacion)
                    .Select(h => new VTAModVentaTHistorialInteraccionDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        IdTipo = h.IdTipo,
                        Detalle = h.Detalle ?? string.Empty,
                        Celular = h.Celular ?? string.Empty,
                        FechaRecordatorio = h.FechaRecordatorio,
                        Estado = h.Estado,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
                    })
                    .ToList();

                respuesta.HistorialInteracciones = lista;
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

        public VTAModVentaTOportunidadDetalleDTORPT ObtenerHistorialEstadoPorOportunidad(int id)
        {
            var respuesta = new VTAModVentaTOportunidadDetalleDTORPT();

            try
            {
                // Todos los Historiales de Estado para la Oportunidad
                var historiales = _unitOfWork.HistorialEstadoRepository
                    .Query()
                    .AsNoTracking()
                    .Where(h => h.IdOportunidad == id)
                    .OrderByDescending(h => h.FechaCreacion)
                    .Select(h => new
                    {
                        h.Id,
                        h.IdOportunidad,
                        h.IdAsesor,
                        h.IdEstado,
                        h.IdOcurrencia,
                        h.Observaciones,
                        h.CantidadLlamadasContestadas,
                        h.CantidadLlamadasNoContestadas,
                        h.Estado,
                        h.FechaCreacion,
                        h.UsuarioCreacion,
                        h.FechaModificacion,
                        h.UsuarioModificacion
                    })
                    .ToList();

                // Historial con Asesor y Estado Referencia
                foreach (var h in historiales)
                {
                    var dto = new VTAModVentaTHistorialEstadoDetalleDTO
                    {
                        Id = h.Id,
                        IdOportunidad = h.IdOportunidad,
                        IdAsesor = h.IdAsesor,
                        IdEstado = h.IdEstado,
                        IdOcurrencia = h.IdOcurrencia,
                        Observaciones = h.Observaciones ?? string.Empty,
                        CantidadLlamadasContestadas = h.CantidadLlamadasContestadas,
                        CantidadLlamadasNoContestadas = h.CantidadLlamadasNoContestadas,
                        Estado = h.Estado,
                        FechaCreacion = h.FechaCreacion,
                        UsuarioCreacion = h.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = h.FechaModificacion,
                        UsuarioModificacion = h.UsuarioModificacion
                    };

                    // Asesor
                    if (h.IdAsesor.HasValue)
                    {
                        var asesor = _unitOfWork.AsesorRepository
                            .Query()
                            .AsNoTracking()
                            .Where(a => a.Id == h.IdAsesor.Value)
                            .Select(a => new { a.Id, a.Nombres, a.Apellidos, a.Correo, a.Celular })
                            .FirstOrDefault();

                        if (asesor != null)
                        {
                            dto.Asesor = new VTAModVentaTAsesorDTO
                            {
                                Id = asesor.Id,
                                Nombres = asesor.Nombres ?? string.Empty,
                                Apellidos = asesor.Apellidos ?? string.Empty,
                                Correo = asesor.Correo ?? string.Empty,
                                Celular = asesor.Celular ?? string.Empty
                            };
                        }
                    }

                    // Estado Referencia
                    if (h.IdEstado.HasValue)
                    {
                        var estadoRef = _unitOfWork.EstadoRepository
                            .Query()
                            .AsNoTracking()
                            .Where(e => e.Id == h.IdEstado.Value)
                            .Select(e => new { e.Id, e.Nombre, e.Descripcion, e.IdTipo, e.EstadoControl, e.FechaCreacion, e.UsuarioCreacion, e.FechaModificacion, e.UsuarioModificacion })
                            .FirstOrDefault();

                        if (estadoRef != null)
                        {
                            string tipoNombre = string.Empty;
                            string tipoCategoria = string.Empty;

                            if (estadoRef.IdTipo > 0)
                            {
                                var tipo = _unitOfWork.TipoRepository
                                    .Query()
                                    .AsNoTracking()
                                    .Where(t => t.Id == estadoRef.IdTipo)
                                    .Select(t => new { t.Id, t.Nombre, t.Categoria })
                                    .FirstOrDefault();

                                if (tipo != null)
                                {
                                    tipoNombre = tipo.Nombre ?? string.Empty;
                                    tipoCategoria = tipo.Categoria ?? string.Empty;
                                }
                            }

                            dto.EstadoReferencia = new VTAModVentaTEstadoDTO
                            {
                                Id = estadoRef.Id,
                                Nombre = estadoRef.Nombre ?? string.Empty,
                                Descripcion = estadoRef.Descripcion ?? string.Empty,
                                IdTipo = estadoRef.IdTipo,
                                TipoNombre = tipoNombre,
                                TipoCategoria = tipoCategoria,
                                Estado = estadoRef.EstadoControl,
                                FechaCreacion = estadoRef.FechaCreacion,
                                UsuarioCreacion = estadoRef.UsuarioCreacion ?? string.Empty,
                                FechaModificacion = estadoRef.FechaModificacion,
                                UsuarioModificacion = estadoRef.UsuarioModificacion ?? string.Empty
                            };
                        }
                    }

                    respuesta.HistorialActual.Add(dto);
                }

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

        public VTAModVentaOportunidadDetalleDTORPT ObtenerTodasOportunidadesRecordatorio()
        {
            var respuesta = new VTAModVentaOportunidadDetalleDTORPT();
            try
            {
                var now = DateTime.UtcNow;
                var sqlDateSentinel = new DateTime(9999, 12, 31, 23, 59, 59, 997);

                var q = _unitOfWork.OportunidadRepository.ObtenerTodas()
                    .Select(o => new
                    {
                        o.Id,
                        o.IdPotencialCliente,
                        PotencialCliente = o.PotencialCliente,
                        PersonaNombres = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Nombres : null,
                        PersonaApellidos = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Apellidos : null,
                        PersonaCorreo = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Correo : null,
                        o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : null,
                        o.CodigoLanzamiento,
                        o.Origen,
                        o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion,

                        // Último historialEstado
                        UltimoHistorial = o.HistorialEstado
                            .OrderByDescending(h => h.FechaCreacion)
                            .ThenByDescending(h => h.Id)
                            .Select(h => new {
                                h.Id,
                                h.IdEstado,
                                h.IdAsesor,
                                h.Observaciones,
                                h.CantidadLlamadasContestadas,
                                h.CantidadLlamadasNoContestadas,
                                h.FechaCreacion,
                                h.UsuarioCreacion,
                                h.IdOcurrencia
                            })
                            .FirstOrDefault(),

                        // HistorialInteraccion tipo 10
                        HistorialInteraccionTipo10 = o.HistorialInteracciones
                            .Where(hi => hi.IdTipo == 10)
                            .OrderBy(hi => (hi.FechaRecordatorio == null) ? sqlDateSentinel : hi.FechaRecordatorio)
                            .ThenByDescending(hi => hi.FechaCreacion)
                            .Select(hi => new { hi.Id, hi.FechaRecordatorio })
                            .FirstOrDefault(),

                        // Calcular TotalOportunidadesPersona
                        TotalOportunidadesPersona = _unitOfWork.OportunidadRepository
                            .Query()
                            .Count(otherO => otherO.IdPotencialCliente == o.IdPotencialCliente)
                    })
                    .ToList();

                var lista = new List<VTAModVentaOportunidadDetalleDTO>();
                var historiales = new List<VTAModVentaTHistorialEstadoDetalleDTO>();

                foreach (var x in q)
                {
                    var dto = new VTAModVentaOportunidadDetalleDTO
                    {
                        Id = x.Id,
                        IdPotencialCliente = x.IdPotencialCliente,
                        PersonaNombre = $"{(x.PersonaNombres ?? "")} {(x.PersonaApellidos ?? "")}".Trim(),
                        PersonaCorreo = x.PersonaCorreo ?? string.Empty,
                        IdProducto = x.IdProducto,
                        ProductoNombre = x.ProductoNombre ?? string.Empty,
                        CodigoLanzamiento = x.CodigoLanzamiento,
                        Origen = x.Origen,
                        Estado = x.Estado,
                        TotalOportunidadesPersona = x.TotalOportunidadesPersona,
                        FechaCreacion = x.FechaCreacion,
                        UsuarioCreacion = x.UsuarioCreacion ?? string.Empty,
                    };

                    if (x.UltimoHistorial != null)
                    {
                        dto.IdHistorialEstado = x.UltimoHistorial.Id;
                        dto.IdEstado = x.UltimoHistorial.IdEstado;
                        dto.NombreEstado = x.UltimoHistorial.IdEstado.HasValue
                            ? (_unitOfWork.EstadoRepository.ObtenerPorId(x.UltimoHistorial.IdEstado.Value)?.Nombre ?? string.Empty)
                            : string.Empty;

                        var histDto = new VTAModVentaTHistorialEstadoDetalleDTO
                        {
                            Id = x.UltimoHistorial.Id,
                            IdOportunidad = x.Id,
                            IdAsesor = x.UltimoHistorial.IdAsesor,
                            IdEstado = x.UltimoHistorial.IdEstado,
                            Observaciones = x.UltimoHistorial.Observaciones ?? string.Empty,
                            CantidadLlamadasContestadas = x.UltimoHistorial.CantidadLlamadasContestadas,
                            CantidadLlamadasNoContestadas = x.UltimoHistorial.CantidadLlamadasNoContestadas,
                            Estado = true,
                            FechaCreacion = x.UltimoHistorial.FechaCreacion,
                            UsuarioCreacion = x.UltimoHistorial.UsuarioCreacion ?? string.Empty,
                        };

                        historiales.Add(histDto);
                    }

                    if (x.HistorialInteraccionTipo10 != null)
                    {
                        dto.IdHistorialInteraccion = x.HistorialInteraccionTipo10.Id;

                        var fr = x.HistorialInteraccionTipo10.FechaRecordatorio;
                        // Si FechaRecordatorio < ahora => NULL, si >= ahora => devolver la fecha
                        dto.FechaRecordatorio = (fr.HasValue && fr.Value.ToUniversalTime() >= now) ? fr.Value : (DateTime?)null;
                    }

                    lista.Add(dto);
                }

                respuesta.Oportunidad = lista;
                respuesta.HistorialActual = historiales;
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

        public VTAModVentaOportunidadDetalleDTO ObtenerOportunidadPorIdConRecordatorio(int id)
        {
            try
            {
                var now = DateTime.UtcNow;

                var x = _unitOfWork.OportunidadRepository.ObtenerTodas()
                    .Where(o => o.Id == id)
                    .Select(o => new
                    {
                        o.Id,
                        o.IdPotencialCliente,
                        PotencialCliente = o.PotencialCliente,
                        PersonaNombres = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Nombres : null,
                        PersonaApellidos = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Apellidos : null,
                        IdPais = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.IdPais : null,
                        PaisNombre = o.PotencialCliente != null && o.PotencialCliente.Persona != null && o.PotencialCliente.Persona.Pais != null ? o.PotencialCliente.Persona.Pais.Nombre : null,
                        CorreoPersona = o.PotencialCliente != null && o.PotencialCliente.Persona != null ? o.PotencialCliente.Persona.Correo : null,
                        o.IdProducto,
                        ProductoNombre = o.Producto != null ? o.Producto.Nombre : null,
                        o.CodigoLanzamiento,
                        o.Estado,
                        FechaCreacion = o.FechaCreacion,
                        UsuarioCreacion = o.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = o.FechaModificacion,
                        UsuarioModificacion = o.UsuarioModificacion,

                        // Último historialEstado
                        UltimoHistorial = o.HistorialEstado
                            .OrderByDescending(h => h.FechaCreacion)
                            .ThenByDescending(h => h.Id)
                            .Select(h => new {
                                h.Id,
                                h.IdEstado,
                                NombreEstado = h.EstadoReferencia != null ? h.EstadoReferencia.Nombre : null
                            })
                            .FirstOrDefault(),

                        // HistorialInteraccion tipo 10
                        HistorialInteraccionTipo10 = o.HistorialInteracciones
                            .Where(hi => hi.IdTipo == 10)
                            .OrderBy(hi => hi.FechaRecordatorio == null ? DateTime.MaxValue : hi.FechaRecordatorio)
                            .ThenByDescending(hi => hi.FechaCreacion)
                            .Select(hi => new { hi.Id, hi.FechaRecordatorio })
                            .FirstOrDefault(),

                        // Calcular TotalOportunidadesPersona
                        TotalOportunidadesPersona = _unitOfWork.OportunidadRepository
                            .Query()
                            .Count(otherO => otherO.IdPotencialCliente == o.IdPotencialCliente)
                    })
                    .FirstOrDefault();

                if (x == null) return new VTAModVentaOportunidadDetalleDTO();

                var dto = new VTAModVentaOportunidadDetalleDTO
                {
                    Id = x.Id,
                    IdPotencialCliente = x.IdPotencialCliente, // Cambiado de IdPersona
                    PersonaNombre = $"{(x.PersonaNombres ?? "")} {(x.PersonaApellidos ?? "")}".Trim(),
                    IdProducto = x.IdProducto,
                    ProductoNombre = x.ProductoNombre ?? string.Empty,
                    PersonaCorreo = x.CorreoPersona ?? string.Empty,
                    CodigoLanzamiento = x.CodigoLanzamiento,
                    Estado = x.Estado,
                    TotalOportunidadesPersona = x.TotalOportunidadesPersona
                };

                if (x.UltimoHistorial != null)
                {
                    dto.IdHistorialEstado = x.UltimoHistorial.Id;
                    dto.IdEstado = x.UltimoHistorial.IdEstado;
                    dto.NombreEstado = x.UltimoHistorial.NombreEstado ?? string.Empty;
                }

                if (x.HistorialInteraccionTipo10 != null)
                {
                    dto.IdHistorialInteraccion = x.HistorialInteraccionTipo10.Id;
                    var fr = x.HistorialInteraccionTipo10.FechaRecordatorio;
                    dto.FechaRecordatorio = (fr.HasValue && fr.Value.ToUniversalTime() >= now) ? fr.Value : (DateTime?)null;
                }

                return dto;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new VTAModVentaOportunidadDetalleDTO();
            }
        }

        public VTAModVentaPotencialClienteDTO ObtenerPotencialPorOportunidadId(int idOportunidad)
        {
            var dto = new VTAModVentaPotencialClienteDTO();

            try
            {
                var resultado = _unitOfWork.OportunidadRepository
                    .Query()
                    .AsNoTracking()
                    .Where(o => o.Id == idOportunidad)
                    .Select(o => o.PotencialCliente)
                    .Where(pc => pc != null)
                    .Select(pc => new VTAModVentaPotencialClienteDTO
                    {
                        Id = pc.Id,
                        IdPersona = pc.IdPersona,
                        Desuscrito = pc.Desuscrito,
                        Estado = pc.Estado,
                        FechaCreacion = pc.FechaCreacion,
                        UsuarioCreacion = pc.UsuarioCreacion ?? string.Empty,
                        FechaModificacion = pc.FechaModificacion,
                        UsuarioModificacion = pc.UsuarioModificacion ?? string.Empty,

                        Persona = pc.Persona == null ? null : new VTAModVentaTPersonaDTO
                        {
                            Id = pc.Persona.Id,
                            IdPais = pc.Persona.IdPais,
                            Pais = pc.Persona.Pais != null ? pc.Persona.Pais.Nombre : string.Empty,
                            Nombres = pc.Persona.Nombres ?? string.Empty,
                            Apellidos = pc.Persona.Apellidos ?? string.Empty,
                            Celular = pc.Persona.Celular ?? string.Empty,
                            PrefijoPaisCelular = pc.Persona.PrefijoPaisCelular ?? string.Empty,
                            Correo = pc.Persona.Correo ?? string.Empty,
                            AreaTrabajo = pc.Persona.AreaTrabajo ?? string.Empty,
                            Industria = pc.Persona.Industria ?? string.Empty,
                            Estado = pc.Persona.Estado,
                            FechaCreacion = pc.Persona.FechaCreacion,
                            UsuarioCreacion = pc.Persona.UsuarioCreacion ?? string.Empty,
                            FechaModificacion = (DateTime?)pc.Persona.FechaModificacion,
                            UsuarioModificacion = pc.Persona.UsuarioModificacion ?? string.Empty
                        }
                    })
                    .FirstOrDefault();

                if (resultado != null)
                    dto = resultado;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }

            return dto;
        }

        public CFGRespuestaGenericaDTO InsertarOportunidadHistorialRegistrado(VTAModVentaTOportunidadDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var persona = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.IdPotencialCliente);
                if (persona == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Persona no encontrada.";
                    return respuesta;
                }

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

                if (dto.FechaRecordatorio == default(DateTime) || string.IsNullOrWhiteSpace(dto.HoraRecordatorio))
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "FechaRecordatorio y HoraRecordatorio no pueden ser nulos.";
                    return respuesta;
                }

                string usuarioCreacion = string.IsNullOrWhiteSpace(dto.UsuarioCreacion) ? "SYSTEM" : dto.UsuarioCreacion;
                string usuarioModificacion = string.IsNullOrWhiteSpace(dto.UsuarioModificacion) ? "SYSTEM" : dto.UsuarioModificacion;
                DateTime nowUtc() => DateTime.UtcNow;

                DateTime BuildFechaRecordatorio()
                {
                    var fecha = dto.FechaRecordatorio; // ya validado no-default
                    var horaStr = dto.HoraRecordatorio ?? string.Empty;

                    if (TimeSpan.TryParse(horaStr, out var ts))
                    {
                        return fecha.Date.Add(ts);
                    }

                    var formatos = new[] { "HH:mm", "H:mm", "HH:mm:ss" };
                    if (DateTime.TryParseExact(horaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtHoraExact))
                    {
                        return fecha.Date.Add(dtHoraExact.TimeOfDay);
                    }

                    if (DateTime.TryParse(horaStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtAny))
                    {
                        return fecha.Date.Add(dtAny.TimeOfDay);
                    }

                    throw new ArgumentException("HoraRecordatorio no tiene un formato válido. Use 'HH:mm' o 'HH:mm:ss'.");
                }

                DateTime fechaRecordatorio;
                try
                {
                    fechaRecordatorio = BuildFechaRecordatorio();
                }
                catch (ArgumentException aex)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = aex.Message;
                    return respuesta;
                }

                var ent = new Oportunidad
                {
                    IdPotencialCliente = dto.IdPotencialCliente,
                    IdProducto = dto.IdProducto,
                    CodigoLanzamiento = dto.CodigoLanzamiento,
                    Origen = dto.Origen,
                    Estado = dto.Estado,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                var historialEstado = new HistorialEstado
                {
                    IdAsesor = 1,
                    IdEstado = 1,
                    IdOcurrencia = 1,
                    Observaciones = "Estado Inicial",
                    CantidadLlamadasContestadas = 0,
                    CantidadLlamadasNoContestadas = 0,
                    Estado = true,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                var historialInteraccion = new HistorialInteraccion
                {
                    IdTipo = 10,
                    Detalle = "Recordatorio inicial de creación de la oportunidad, generado automaticamente",
                    Celular = null,
                    FechaRecordatorio = fechaRecordatorio,
                    Estado = true,
                    FechaCreacion = nowUtc(),
                    UsuarioCreacion = usuarioCreacion,
                    FechaModificacion = nowUtc(),
                    UsuarioModificacion = usuarioModificacion
                };

                DbContext? dbContext = null;
                var uwType = _unitOfWork.GetType();
                var propCandidates = new[] { "Context", "_context", "DbContext", "Contexto", "ContextoDb" };
                foreach (var pName in propCandidates)
                {
                    var prop = uwType.GetProperty(pName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (prop != null)
                    {
                        dbContext = prop.GetValue(_unitOfWork) as DbContext;
                        if (dbContext != null) break;
                    }
                }

                if (dbContext != null)
                {
                    using (var tx = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _unitOfWork.OportunidadRepository.Insertar(ent);
                            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                            historialEstado.IdOportunidad = ent.Id;
                            historialInteraccion.IdOportunidad = ent.Id;

                            _unitOfWork.HistorialEstadoRepository.Insertar(historialEstado);
                            _unitOfWork.HistorialInteraccionRepository.Insertar(historialInteraccion);
                            _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                            tx.Commit();

                            respuesta.Codigo = SR._C_SIN_ERROR;
                            respuesta.Mensaje = string.Empty;
                        }
                        catch (Exception exInner)
                        {
                            try { tx.Rollback(); } catch { }
                            throw;
                        }
                    }
                }
                else
                {
                    // sin DbContext accesible en el UoW: persistir de forma secuencial
                    _unitOfWork.OportunidadRepository.Insertar(ent);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                    historialEstado.IdOportunidad = ent.Id;
                    historialInteraccion.IdOportunidad = ent.Id;

                    _unitOfWork.HistorialEstadoRepository.Insertar(historialEstado);
                    _unitOfWork.HistorialInteraccionRepository.Insertar(historialInteraccion);
                    _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

                    respuesta.Codigo = SR._C_SIN_ERROR;
                    respuesta.Mensaje = string.Empty;
                }
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
