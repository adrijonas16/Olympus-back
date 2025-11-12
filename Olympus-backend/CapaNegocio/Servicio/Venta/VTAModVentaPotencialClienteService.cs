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
    public class VTAModVentaPotencialClienteService : IVTAModVentaPotencialClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;

        public VTAModVentaPotencialClienteService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        public PotencialClienteDTORPT ObtenerTodas()
        {
            var respuesta = new PotencialClienteDTORPT();
            try
            {
                var lista = _unitOfWork.PotencialClienteRepository.ObtenerTodos()
                    .Select(p => new VTAModVentaPotencialClienteDTO
                    {
                        Id = p.Id,
                        IdPersona = p.IdPersona,
                        Desuscrito = p.Desuscrito,
                        Estado = p.Estado,
                        FechaCreacion = p.FechaCreacion,
                        UsuarioCreacion = p.UsuarioCreacion,
                        FechaModificacion = p.FechaModificacion,
                        UsuarioModificacion = p.UsuarioModificacion
                    })
                    .ToList();

                respuesta.PotencialClientes = lista;
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

        public VTAModVentaPotencialClienteDTO ObtenerPorId(int id)
        {
            var dto = new VTAModVentaPotencialClienteDTO();
            try
            {
                var ent = _unitOfWork.PotencialClienteRepository.ObtenerPorId(id);
                if (ent != null)
                {
                    dto.Id = ent.Id;
                    dto.IdPersona = ent.IdPersona;
                    dto.Desuscrito = ent.Desuscrito;
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

        public CFGRespuestaGenericaDTO Insertar(VTAModVentaPotencialClienteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = new PotencialCliente
                {
                    IdPersona = dto.IdPersona,
                    Desuscrito = dto.Desuscrito,
                    Estado = dto.Estado,
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = "SYSTEM",
                    FechaModificacion = DateTime.UtcNow,
                    UsuarioModificacion = "SYSTEM"
                };

                _unitOfWork.PotencialClienteRepository.Insertar(ent);
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

        public CFGRespuestaGenericaDTO Actualizar(VTAModVentaPotencialClienteDTO dto)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ent = _unitOfWork.PotencialClienteRepository.ObtenerPorId(dto.Id);
                if (ent == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = SR._M_NO_ENCONTRADO;
                    return respuesta;
                }

                ent.IdPersona = dto.IdPersona;
                ent.Desuscrito = dto.Desuscrito;
                ent.Estado = dto.Estado;
                ent.FechaModificacion = DateTime.UtcNow;
                ent.UsuarioModificacion = "SYSTEM";

                _unitOfWork.PotencialClienteRepository.Actualizar(ent);
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
                var success = _unitOfWork.PotencialClienteRepository.Eliminar(id);
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
