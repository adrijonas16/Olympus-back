using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
                var lista = _unitOfWork.ProductoRepository
                    .Query()
                    .AsNoTracking()
                    .Select(p => new VTAModVentaProductoDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        CodigoLanzamiento = p.CodigoLanzamiento,
                        FechaInicio = p.FechaInicio,
                        FechaPresentacion = p.FechaPresentacion,
                        DatosImportantes = p.DatosImportantes,
                        Estado = p.Estado,
                        FechaCreacion = p.FechaCreacion,
                        UsuarioCreacion = p.UsuarioCreacion,
                        FechaModificacion = p.FechaModificacion,
                        UsuarioModificacion = p.UsuarioModificacion
                    })
                    .ToList();

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
                var ent = _unitOfWork.ProductoRepository
                    .Query()
                    .AsNoTracking()
                    .Where(p => p.Id == id)
                    .Select(p => new VTAModVentaProductoDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        CodigoLanzamiento = p.CodigoLanzamiento,
                        FechaInicio = p.FechaInicio,
                        FechaPresentacion = p.FechaPresentacion,
                        DatosImportantes = p.DatosImportantes,
                        Estado = p.Estado,
                        FechaCreacion = p.FechaCreacion,
                        UsuarioCreacion = p.UsuarioCreacion,
                        FechaModificacion = p.FechaModificacion,
                        UsuarioModificacion = p.UsuarioModificacion
                    })
                    .FirstOrDefault();

                if (ent != null)
                {
                    dto = ent;
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
                    Nombre = dto.Nombre ?? string.Empty,
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

        public VTAModVentaProductoDetalleRPT ObtenerDetallePorOportunidad(int idOportunidad)
        {
            var respuesta = new VTAModVentaProductoDetalleRPT();
            try
            {
                var dbContext = _unitOfWork.Context;
                var conn = dbContext.Database.GetDbConnection();

                if (conn.State != ConnectionState.Open) conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SPProductoDetallePorOportunidad";

                var param = cmd.CreateParameter();
                param.ParameterName = "@IdOportunidad";
                param.DbType = DbType.Int32;
                param.Value = idOportunidad;
                cmd.Parameters.Add(param);

                using var reader = cmd.ExecuteReader();

                var ordinals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                object GetVal(DbDataReader r, string col)
                {
                    try
                    {
                        if (!ordinals.TryGetValue(col, out var idx))
                        {
                            idx = r.GetOrdinal(col);
                            ordinals[col] = idx;
                        }
                        if (r.IsDBNull(idx)) return null!;
                        return r.GetValue(idx);
                    }
                    catch
                    {
                        return null!;
                    }
                }

                // 1) Producto (primer resultset)
                if (reader.HasRows && reader.Read())
                {
                    respuesta.Producto = new VTAModVentaProductoDTO
                    {
                        Id = GetVal(reader, "IdProducto") as int? ?? 0,
                        Nombre = GetVal(reader, "Nombre")?.ToString() ?? string.Empty,
                        CodigoLanzamiento = GetVal(reader, "CodigoLanzamiento")?.ToString() ?? string.Empty,
                        FechaInicio = GetVal(reader, "FechaInicio") as DateTime?,
                        FechaPresentacion = GetVal(reader, "FechaPresentacion") as DateTime?,
                        DatosImportantes = GetVal(reader, "DatosImportantes")?.ToString() ?? string.Empty,
                    };
                }

                // 2) Horarios
                if (reader.NextResult())
                {
                    var horarios = new List<VTAModVentaHorarioDTO>();
                    while (reader.Read())
                    {
                        TimeSpan? horaInicio = null;
                        TimeSpan? horaFin = null;
                        var rawHoraInicio = GetVal(reader, "HoraInicio");
                        var rawHoraFin = GetVal(reader, "HoraFin");

                        if (rawHoraInicio != null)
                        {
                            if (rawHoraInicio is TimeSpan ts1) horaInicio = ts1;
                            else if (TimeSpan.TryParse(rawHoraInicio.ToString(), out var t1)) horaInicio = t1;
                            else if (DateTime.TryParse(rawHoraInicio.ToString(), out var dt1)) horaInicio = dt1.TimeOfDay;
                        }

                        if (rawHoraFin != null)
                        {
                            if (rawHoraFin is TimeSpan ts2) horaFin = ts2;
                            else if (TimeSpan.TryParse(rawHoraFin.ToString(), out var t2)) horaFin = t2;
                            else if (DateTime.TryParse(rawHoraFin.ToString(), out var dt2)) horaFin = dt2.TimeOfDay;
                        }

                        horarios.Add(new VTAModVentaHorarioDTO
                        {
                            Id = GetVal(reader, "IdHorario") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            Dia = GetVal(reader, "Dia")?.ToString() ?? string.Empty,
                            HoraInicio = horaInicio,
                            HoraFin = horaFin,
                            Detalle = GetVal(reader, "Detalle")?.ToString() ?? string.Empty,
                            Orden = GetVal(reader, "Orden") as int?,
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty
                        });
                    }
                    respuesta.Horarios = horarios;
                }

                // 3) Inversiones
                if (reader.NextResult())
                {
                    var inv = new List<VTAModVentaInversionDTO>();
                    while (reader.Read())
                    {
                        inv.Add(new VTAModVentaInversionDTO
                        {
                            Id = GetVal(reader, "IdInversion") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            IdOportunidad = GetVal(reader, "IdOportunidad") as int? ?? 0,
                            CostoTotal = GetVal(reader, "CostoTotal") as decimal? ?? 0m,
                            Moneda = GetVal(reader, "Moneda")?.ToString() ?? string.Empty,
                            DescuentoPorcentaje = GetVal(reader, "DescuentoPorcentaje") as decimal?,
                            DescuentoMonto = GetVal(reader, "DescuentoMonto") as decimal?,
                            CostoOfrecido = GetVal(reader, "CostoOfrecido") as decimal?,
                            Estado = GetVal(reader, "Estado") as bool? ?? true,
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty
                        });
                    }
                    respuesta.Inversiones = inv;
                }

                // 4) Estructuras
                if (reader.NextResult())
                {
                    var ecs = new List<VTAModVentaEstructuraCurricularDTO>();
                    while (reader.Read())
                    {
                        ecs.Add(new VTAModVentaEstructuraCurricularDTO
                        {
                            Id = GetVal(reader, "IdEstructuraCurricular") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            Nombre = GetVal(reader, "NombreEstructura")?.ToString() ?? string.Empty,
                            Descripcion = GetVal(reader, "DescripcionEstructura")?.ToString(),
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty
                        });
                    }
                    respuesta.Estructuras = ecs;
                }

                // 5) Módulos
                if (reader.NextResult())
                {
                    var mods = new List<VTAModVentaEstructuraCurricularModuloDTO>();
                    while (reader.Read())
                    {
                        var dto = new VTAModVentaEstructuraCurricularModuloDTO
                        {
                            Id = GetVal(reader, "IdEstructuraCurricularModulo") as int? ?? 0,
                            IdEstructuraCurricular = GetVal(reader, "IdEstructuraCurricular") as int? ?? 0,
                            IdModulo = GetVal(reader, "IdModulo") as int? ?? 0,
                            Orden = GetVal(reader, "Orden") as int?,
                            Sesiones = GetVal(reader, "Sesiones") as int?,
                            DuracionHoras = GetVal(reader, "DuracionHorasEnEstructura") as int? ?? (GetVal(reader, "DuracionHorasModulo") as int?),
                            Observaciones = GetVal(reader, "Observaciones")?.ToString()
                        };

                        dto.Modulo = new VTAModVentaModuloDTO
                        {
                            Id = GetVal(reader, "IdModulo") as int? ?? 0,
                            Nombre = GetVal(reader, "NombreModulo")?.ToString() ?? string.Empty,
                            Codigo = GetVal(reader, "CodigoModulo")?.ToString(),
                            Descripcion = GetVal(reader, "DescripcionModulo")?.ToString(),
                            DuracionHoras = GetVal(reader, "DuracionHorasModulo") as int?
                        };

                        mods.Add(dto);
                    }
                    respuesta.EstructuraModulos = mods;
                }

                // 6) Docentes por módulo
                if (reader.NextResult())
                {
                    var docs = new List<VTAModVentaEstructuraCurricularModuloDTO>();
                    while (reader.Read())
                    {
                        var dto = new VTAModVentaEstructuraCurricularModuloDTO
                        {
                            IdEstructuraCurricular = GetVal(reader, "IdEstructuraCurricularModulo") as int? ?? 0,
                            IdModulo = GetVal(reader, "IdModulo") as int? ?? 0,
                            IdDocente = GetVal(reader, "IdDocente") as int?,
                            IdPersonaDocente = GetVal(reader, "IdPersona") as int?,
                            DocenteNombre = (GetVal(reader, "Nombres")?.ToString() ?? string.Empty) + " " + (GetVal(reader, "Apellidos")?.ToString() ?? string.Empty)
                        };
                        docs.Add(dto);
                    }
                    respuesta.DocentesPorModulo = docs;
                }

                // 7) ProductoCertificados
                if (reader.NextResult())
                {
                    var pcs = new List<VTAModVentaProductoCertificadoDTO>();
                    while (reader.Read())
                    {
                        pcs.Add(new VTAModVentaProductoCertificadoDTO
                        {
                            Id = GetVal(reader, "IdProductoCertificado") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            IdCertificado = GetVal(reader, "IdCertificado") as int? ?? 0,
                            NombreCertificado = GetVal(reader, "NombreCertificado")?.ToString() ?? string.Empty, // <--- nombre
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty,
                        });
                    }
                    respuesta.ProductoCertificados = pcs;
                }

                // 8) MetodoPagoProducto
                if (reader.NextResult())
                {
                    var mpps = new List<VTAModVentaMetodoPagoProductoDTO>();
                    while (reader.Read())
                    {
                        mpps.Add(new VTAModVentaMetodoPagoProductoDTO
                        {
                            Id = GetVal(reader, "IdMetodoPagoProducto") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            IdMetodoPago = GetVal(reader, "IdMetodoPago") as int? ?? 0,
                            NombreMetodoPago = GetVal(reader, "NombreMetodoPago")?.ToString() ?? string.Empty, // <--- nombre
                            Activo = GetVal(reader, "Activo") as bool? ?? true,
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty
                        });
                    }
                    respuesta.MetodosPago = mpps;
                }

                // 9) Beneficios
                if (reader.NextResult())
                {
                    var ben = new List<VTAModVentaBeneficioDTO>();
                    while (reader.Read())
                    {
                        ben.Add(new VTAModVentaBeneficioDTO
                        {
                            Id = GetVal(reader, "IdBeneficio") as int? ?? 0,
                            IdProducto = GetVal(reader, "IdProducto") as int? ?? 0,
                            Descripcion = GetVal(reader, "Descripcion")?.ToString() ?? string.Empty,
                            Orden = GetVal(reader, "Orden") as int?,
                            FechaCreacion = GetVal(reader, "FechaCreacion") as DateTime? ?? default,
                            UsuarioCreacion = GetVal(reader, "UsuarioCreacion")?.ToString() ?? string.Empty
                        });
                    }
                    respuesta.Beneficios = ben;
                }

                reader.Close();

                respuesta.Codigo = SR._C_SIN_ERROR;
                respuesta.Mensaje = string.Empty;
                return respuesta;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                return new VTAModVentaProductoDetalleRPT
                {
                    Codigo = SR._C_ERROR_CRITICO,
                    Mensaje = ex.Message
                };
            }
        }

    }
}
