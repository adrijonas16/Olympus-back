using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;
using System;
using System.Collections.Generic;
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

        public VTAModVentaEstadoTransicionService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _errorLogService = errorLogService;
        }

        // DTO interno para devolver ocurrencias con flag Allowed
        public class OcurrenciaDto
        {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public int? IdEstado { get; set; }
            public bool Allowed { get; set; }
        }

        public List<OcurrenciaDto> ObtenerOcurrenciasPermitidas(int oportunidadId)
        {
            var resultado = new List<OcurrenciaDto>();
            try
            {
                // 1) obtener estado actual de la oportunidad (último historial)
                var ultimoHistorial = _unitOfWork.HistorialEstadoRepository.ObtenerTodos()
                    .Where(h => h.IdOportunidad == oportunidadId)
                    .OrderByDescending(h => h.FechaCreacion)
                    .FirstOrDefault();

                int? estadoActual = ultimoHistorial?.IdEstado;
                int? ocurrenciaOrigenId = ultimoHistorial?.IdOcurrencia;


                // 2) obtener todas las ocurrencias con su estado
                var ocurrencias = _unitOfWork.OcurrenciaRepository.ObtenerTodos()
                    .Select(o => new OcurrenciaDto
                    {
                        Id = o.Id,
                        Nombre = o.Nombre,
                        IdEstado = o.IdEstado
                    })
                    .ToList();

                // 3) obtener reglas relevantes (traer reglas que podrían aplicar)
                var posiblesToEstados = ocurrencias.Select(o => o.IdEstado).Where(x => x.HasValue).Select(x => x!.Value).Distinct().ToList();

                var reglas = _unitOfWork.EstadoTransicionRepository.ObtenerTodos()
                    .Where(r =>
                        (r.IdEstadoOrigen == estadoActual || r.IdEstadoOrigen == null)
                        || (r.IdEstadoDestino == null || posiblesToEstados.Contains(r.IdEstadoDestino ?? -999))
                        || (r.IdOcurrenciaDestino != null) || (r.IdOcurrenciaOrigen != null)
                    ).ToList();

                // 4) evaluar por cada ocurrencia
                foreach (var oc in ocurrencias)
                {
                    int? targetEstado = oc.IdEstado;
                    bool? decision = null;

                    // 1) coincidencia exacta: estadoOrigen + estadoDestino + ocurrenciaOrigen + ocurrenciaDestino
                    var regla = reglas.FirstOrDefault(r =>
                        r.IdEstadoOrigen == estadoActual &&
                        r.IdEstadoDestino == targetEstado &&
                        r.IdOcurrenciaOrigen == ocurrenciaOrigenId &&
                        r.IdOcurrenciaDestino == oc.Id);

                    // 2) estadoOrigen + estadoDestino + ocurrenciaDestino
                    if (regla == null)
                        regla = reglas.FirstOrDefault(r =>
                            r.IdEstadoOrigen == estadoActual &&
                            r.IdEstadoDestino == targetEstado &&
                            r.IdOcurrenciaDestino == oc.Id);

                    // 3) estadoOrigen + estadoDestino
                    if (regla == null)
                        regla = reglas.FirstOrDefault(r =>
                            r.IdEstadoOrigen == estadoActual &&
                            r.IdEstadoDestino == targetEstado);

                    // 4) estadoOrigen + ocurrenciaOrigen + ocurrenciaDestino
                    if (regla == null && ocurrenciaOrigenId != null)
                        regla = reglas.FirstOrDefault(r =>
                            r.IdEstadoOrigen == estadoActual &&
                            r.IdOcurrenciaOrigen == ocurrenciaOrigenId &&
                            r.IdOcurrenciaDestino == oc.Id);

                    // 5) estadoOrigen solo (origen-only)
                    if (regla == null)
                        regla = reglas.FirstOrDefault(r => r.IdEstadoOrigen == estadoActual && r.IdOcurrenciaDestino == null && r.IdEstadoDestino == null);

                    // 6) ocurrenciaDestino-only
                    if (regla == null)
                        regla = reglas.FirstOrDefault(r => r.IdOcurrenciaDestino == oc.Id && r.IdEstadoOrigen == null && r.IdEstadoDestino == null);

                    // 7) regla global (NULL,NULL)
                    if (regla == null)
                        regla = reglas.FirstOrDefault(r => r.IdEstadoOrigen == null && r.IdEstadoDestino == null && r.IdOcurrenciaOrigen == null && r.IdOcurrenciaDestino == null);

                    if (regla != null)
                        decision = regla.Permitido;

                    // En caso final permitir todo, pero evitar transición a mismo estado
                    if (decision == null)
                    {
                        if (estadoActual != null && targetEstado != null && estadoActual == targetEstado)
                            oc.Allowed = false;
                        else
                            oc.Allowed = true;
                    }
                    else
                    {
                        oc.Allowed = decision.Value;
                    }

                    resultado.Add(oc);
                }
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                // En caso de error lista vacía
            }

            return resultado;
        }

        /// <summary>
        /// Crea un nuevo HistorialEstado a partir de la ocurrencia seleccionada.
        /// Valida reglas antes de insertar.
        /// </summary>
        public CFGRespuestaGenericaDTO CrearHistorialConOcurrencia(int oportunidadId, int ocurrenciaId, string usuario)
        {
            var respuesta = new CFGRespuestaGenericaDTO();
            try
            {
                var ultimo = _unitOfWork.HistorialEstadoRepository.ObtenerTodos()
                    .Where(h => h.IdOportunidad == oportunidadId)
                    .OrderByDescending(h => h.FechaCreacion)
                    .FirstOrDefault();

                int? fromEstado = ultimo?.IdEstado;

                var ocurrencia = _unitOfWork.OcurrenciaRepository
                .Query() // IQueryable<Ocurrencia>
                .AsNoTracking()
                .Where(o => o.Id == ocurrenciaId)
                .Select(o => new {
                    o.Id,
                    o.Nombre,
                    o.IdEstado
                })
                .FirstOrDefault();
                if (ocurrencia == null)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Ocurrencia no existe.";
                    return respuesta;
                }

                int? toEstado = ocurrencia.IdEstado;

                int? ocurrenciaOrigenId = ultimo?.IdOcurrencia;
                bool allowed = IsTransitionAllowed(fromEstado, toEstado, ocurrenciaOrigenId, ocurrenciaId);
                if (!allowed)
                {
                    respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                    respuesta.Mensaje = "Transición no permitida según reglas de negocio.";
                    return respuesta;
                }

                var nuevo = new HistorialEstado
                {
                    IdOportunidad = oportunidadId,
                    IdEstado = toEstado,
                    IdOcurrencia = ocurrenciaId,
                    Observaciones = $"Cambio por ocurrencia {ocurrencia.Nombre}",
                    FechaCreacion = DateTime.UtcNow,
                    UsuarioCreacion = string.IsNullOrWhiteSpace(usuario) ? "SYSTEM" : usuario,
                    Estado = true
                };

                _unitOfWork.HistorialEstadoRepository.Insertar(nuevo);
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

        /// <summary>
        /// Evalúa si la transición fromEstado -> toEstado está permitida según las reglas en adm.EstadoTransicion.
        /// Si no hay regla explícita, se permite por defecto salvo que from==to.
        /// </summary>
        private bool IsTransitionAllowed(int? fromEstado, int? toEstado, int? ocurrenciaOrigenId = null, int? ocurrenciaDestinoId = null)
        {
            try
            {
                var reglas = _unitOfWork.EstadoTransicionRepository.ObtenerTodos().ToList();

                var regla = reglas.FirstOrDefault(r =>
                    r.IdEstadoOrigen == fromEstado &&
                    r.IdEstadoDestino == toEstado &&
                    r.IdOcurrenciaOrigen == ocurrenciaOrigenId &&
                    r.IdOcurrenciaDestino == ocurrenciaDestinoId)
                    ??
                    reglas.FirstOrDefault(r =>
                    r.IdEstadoOrigen == fromEstado &&
                    r.IdEstadoDestino == toEstado &&
                    r.IdOcurrenciaDestino == ocurrenciaDestinoId)
                    ??
                    reglas.FirstOrDefault(r =>
                    r.IdEstadoOrigen == fromEstado &&
                    r.IdEstadoDestino == toEstado)
                    ??
                    (ocurrenciaOrigenId != null ? reglas.FirstOrDefault(r =>
                        r.IdEstadoOrigen == fromEstado &&
                        r.IdOcurrenciaOrigen == ocurrenciaOrigenId &&
                        r.IdOcurrenciaDestino == ocurrenciaDestinoId) : null)
                    ??
                    reglas.FirstOrDefault(r => r.IdEstadoOrigen == fromEstado && r.IdOcurrenciaDestino == ocurrenciaDestinoId)
                    ??
                    reglas.FirstOrDefault(r => r.IdOcurrenciaDestino == ocurrenciaDestinoId)
                    ??
                    reglas.FirstOrDefault(r => r.IdEstadoOrigen == null && r.IdEstadoDestino == null && r.IdOcurrenciaOrigen == null && r.IdOcurrenciaDestino == null);

                if (regla != null)
                    return regla.Permitido;

                if (fromEstado != null && toEstado != null && fromEstado == toEstado) return false;
                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.RegistrarError(ex);
                // En caso de error  bloquear la transición
                return false;
            }
        }
    }

}
