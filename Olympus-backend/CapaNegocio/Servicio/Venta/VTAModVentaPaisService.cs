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
    public class VTAModVentaPaisService : IVTAModVentaPaisService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaPaisService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public VTAModVentaTPaisDTORPT ObtenerTodas()
        {
            var respuesta = new VTAModVentaTPaisDTORPT();
            try
            {
                var lista = _unitOfWork.PaisRepository.ObtenerTodos()
                    .Select(p => new VTAModVentaTPaisDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        PrefijoCelularPais = p.PrefijoCelularPais,
                        DigitoMaximo = p.DigitoMaximo,
                        DigitoMinimo = p.DigitoMinimo,
                        Estado = p.Estado,
                        UsuarioCreacion = p.UsuarioCreacion,
                        FechaCreacion = p.FechaCreacion,
                        UsuarioModificacion = p.UsuarioModificacion,
                        FechaModificacion = p.FechaModificacion
                    })
                    .ToList();

                respuesta.Pais = lista;
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

        public VTAModVentaTPaisDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaTPaisDTO();
            try
            {
                var ent = _unitOfWork.PaisRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.Nombre = ent.Nombre;
                    dto.PrefijoCelularPais = ent.PrefijoCelularPais;
                    dto.DigitoMaximo = ent.DigitoMaximo;
                    dto.DigitoMinimo = ent.DigitoMinimo;
                    dto.Estado = ent.Estado;
                    dto.UsuarioCreacion = ent.UsuarioCreacion;
                    dto.FechaCreacion = ent.FechaCreacion;
                    dto.UsuarioModificacion = ent.UsuarioModificacion;
                    dto.FechaModificacion = ent.FechaModificacion;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
            }
            return dto;
        }

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaTPaisDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new Pais
                {
                    Nombre = dto.Nombre,
                    PrefijoCelularPais = dto.PrefijoCelularPais,
                    DigitoMaximo = dto.DigitoMaximo,
                    DigitoMinimo = dto.DigitoMinimo,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.PaisRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaTPaisDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.PaisRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.Nombre = dto.Nombre;
                ent.PrefijoCelularPais = dto.PrefijoCelularPais;
                ent.DigitoMaximo = dto.DigitoMaximo;
                ent.DigitoMinimo = dto.DigitoMinimo;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.PaisRepository.Actualizar(ent);
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
                var success = _unitOfWork.PaisRepository.Eliminar(id);
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
