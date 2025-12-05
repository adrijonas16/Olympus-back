using CapaDatos.DataContext;
using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaEstadoTransicionService : IVTAModVentaEstadoTransicionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IErrorLogService _errorLogService;
        private readonly OlympusContext _context;

        public VTAModVentaEstadoTransicionService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService, OlympusContext context)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
            _context = context;
        }

        public VTAModVentaOcurrenciasPermitidasDTORPT ObtenerOcurrenciasPermitidas(int IdOportunidad)
        {
            var respuesta = new VTAModVentaOcurrenciasPermitidasDTORPT();
            try
            {
                var lista = new List<VTAModVentaOcurrenciaSimpleDTO>();
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "adm.SP_ObtenerOcurrenciasPermitidasPorOportunidad";
                cmd.CommandType = CommandType.StoredProcedure;

                // Parametro
                var p = new SqlParameter("@IdOportunidad", SqlDbType.Int) { Value = IdOportunidad };
                cmd.Parameters.Add(p);

                if (conn.State != ConnectionState.Open) conn.Open();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var oc = new VTAModVentaOcurrenciaSimpleDTO
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                        IdEstado = reader.IsDBNull(reader.GetOrdinal("IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdEstado")),
                        Allowed = reader.IsDBNull(reader.GetOrdinal("Allowed")) ? false : reader.GetBoolean(reader.GetOrdinal("Allowed"))
                    };

                    lista.Add(oc);
                }

                respuesta.Ocurrencias = lista;
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
        public VTAModVentaOcurrenciasPermitidasDTORPT ObtenerOcurrenciasPermitidas2(int IdOportunidad, int IdUsuario, int IdRol)
        {
            var respuesta = new VTAModVentaOcurrenciasPermitidasDTORPT();

            try
            {
                // 1) Obtener la oportunidad
                var op = _unitOfWork.OportunidadRepository.ObtenerPorId(IdOportunidad);

                if (op == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "La oportunidad no existe.";
                    return respuesta;
                }

                // 2) Obtener la persona asociada al usuario (si existe)
                // Asumimos que existe un repositorio o método para obtener Persona por IdUsuario.
                // Si tu repo tiene otro nombre, cámbialo aquí.
                var personaUsuario = _unitOfWork.PersonaRepository.ObtenerPorIdUsuario(IdUsuario);
                // personaUsuario puede ser null si no hay persona ligada al usuario.

                bool tienePermiso = false;

                // 3) Permiso si la persona del usuario es la misma que la persona asociada a la oportunidad
                // Nota: op.IdPersona es la FK que mencionaste (la oportunidad guarda IdPersona)
                if (personaUsuario != null && op.IdPersona == personaUsuario.Id)
                {
                    tienePermiso = true;
                }

                // 4) Permiso si el usuario es el asesor asignado a la oportunidad
                // Dependiendo de tu modelo, op.IdAsesor puede contener la IdPersona del asesor.
                // Compararemos con personaUsuario.Id si existe.
                if (!tienePermiso && personaUsuario != null && op.IdPersona.HasValue && op.IdPersona.Value == personaUsuario.Id)
                {
                    tienePermiso = true;
                }

                // 5) Permiso por rol (Supervisor, Gerente, Administrador, Desarrollador)
                if (!tienePermiso && (IdRol == 2 || IdRol == 3 || IdRol == 4 || IdRol == 5))
                {
                    tienePermiso = true;
                }

                if (!tienePermiso)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "No tienes permiso para ver esta oportunidad.";
                    return respuesta;
                }

                // 6) Si tiene permiso -> ejecutar SP para obtener ocurrencias permitidas
                var lista = new List<VTAModVentaOcurrenciaSimpleDTO>();
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "adm.SP_ObtenerOcurrenciasPermitidasPorOportunidad";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdOportunidad", IdOportunidad));

                if (conn.State != ConnectionState.Open) conn.Open();

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new VTAModVentaOcurrenciaSimpleDTO
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                        IdEstado = reader.IsDBNull(reader.GetOrdinal("IdEstado")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdEstado")),
                        Allowed = reader.IsDBNull(reader.GetOrdinal("Allowed")) ? false : reader.GetBoolean(reader.GetOrdinal("Allowed"))
                    });
                }

                respuesta.Ocurrencias = lista;
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



        /// <summary>
        /// Crea un nuevo HistorialEstado a partir de la ocurrencia seleccionada.
        /// </summary>
        public (CFGRespuestaGenericaDTO Respuesta, int NuevoHistorialId) CrearHistorialConOcurrencia(int oportunidadId, int ocurrenciaId, string usuario)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            int nuevoHistorialId = 0;

            try
            {
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "adm.SP_CrearHistorialConOcurrencia";

                cmd.Parameters.Add(new SqlParameter("@IdOportunidad", SqlDbType.Int) { Value = oportunidadId });
                cmd.Parameters.Add(new SqlParameter("@IdOcurrenciaDestino", SqlDbType.Int) { Value = ocurrenciaId });
                cmd.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = string.IsNullOrWhiteSpace(usuario) ? "SYSTEM" : usuario });

                var pOutCodigo = new SqlParameter("@OutCodigo", SqlDbType.NVarChar, 50) { Direction = ParameterDirection.Output };
                var pOutMensaje = new SqlParameter("@OutMensaje", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };
                var pOutNewId = new SqlParameter("@OutNewHistorialId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(pOutCodigo);
                cmd.Parameters.Add(pOutMensaje);
                cmd.Parameters.Add(pOutNewId);

                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();

                var outCodigo = pOutCodigo.Value == DBNull.Value ? string.Empty : pOutCodigo.Value.ToString() ?? string.Empty;
                var outMensaje = pOutMensaje.Value == DBNull.Value ? string.Empty : pOutMensaje.Value.ToString() ?? string.Empty;
                nuevoHistorialId = (pOutNewId.Value == DBNull.Value) ? 0 : Convert.ToInt32(pOutNewId.Value);

                if (string.Equals(outCodigo, "OK", StringComparison.OrdinalIgnoreCase))
                    respuesta.Codigo = SR._C_SIN_ERROR;
                else if (string.Equals(outCodigo, "ERROR_CONTROLADO", StringComparison.OrdinalIgnoreCase))
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                else
                    respuesta.Codigo = outCodigo;

                respuesta.Mensaje = outMensaje ?? string.Empty;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                respuesta.Codigo = SR._C_ERROR_CRITICO;
                respuesta.Mensaje = ex.Message;
            }

            return (respuesta, nuevoHistorialId);
        }
    }

}
