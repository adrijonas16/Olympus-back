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
using System.Data;
using System.Linq;

namespace CapaNegocio.Servicio.Venta
{
    public class VTAModVentaCobranzaService : IVTAModVentaCobranzaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly OlympusContext _context;
        private readonly IErrorLogService _errorLog;

        public VTAModVentaCobranzaService(IUnitOfWork unitOfWork, OlympusContext context, IErrorLogService errorLog)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _errorLog = errorLog;
        }

        public int CrearPlanCobranza(VTAModVentaCobranzaCrearPlanDTO dto)
        {
            try
            {
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "adm.SPCrearPlanCobranza";
                cmd.CommandType = CommandType.StoredProcedure;

                var p_IdOport = new SqlParameter("@IdOportunidad", SqlDbType.Int) { Value = dto.IdOportunidad };
                var p_Total = new SqlParameter("@Total", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = dto.Total };
                var p_Num = new SqlParameter("@NumCuotas", SqlDbType.Int) { Value = dto.NumCuotas };
                var p_FechaInicio = new SqlParameter("@FechaInicio", SqlDbType.DateTime) { Value = dto.FechaInicio };
                var p_Frec = new SqlParameter("@FrecuenciaDias", SqlDbType.Int) { Value = dto.FrecuenciaDias };
                var p_Usuario = new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = dto.Usuario };
                var p_NewPlanId = new SqlParameter("@NewPlanId", SqlDbType.Int) { Direction = ParameterDirection.Output };

                cmd.Parameters.Add(p_IdOport);
                cmd.Parameters.Add(p_Total);
                cmd.Parameters.Add(p_Num);
                cmd.Parameters.Add(p_FechaInicio);
                cmd.Parameters.Add(p_Frec);
                cmd.Parameters.Add(p_Usuario);
                cmd.Parameters.Add(p_NewPlanId);

                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();

                var newId = (int)(p_NewPlanId.Value ?? 0);
                return newId;
            }
            catch (Exception ex)
            {
                _errorLog.RegistrarError(ex);
                throw;
            }
        }

        public int RegistrarPago(VTAModVentaCobranzaPagoRegistroDTO dto, bool usarAcumulada = false)
        {
            try
            {
                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();

                if (usarAcumulada)
                    cmd.CommandText = "adm.SPRegistroPagoCobranzaCuotaAcumulada";
                else
                    cmd.CommandText = "adm.SPRegistroPagoCobranza";

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdCobranzaPlan", SqlDbType.Int) { Value = dto.IdCobranzaPlan });
                cmd.Parameters.Add(new SqlParameter("@IdCuotaInicial", SqlDbType.Int) { Value = (object?)dto.IdCuotaInicial ?? DBNull.Value });
                var pMonto = new SqlParameter("@MontoPago", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = dto.MontoPago };
                cmd.Parameters.Add(pMonto);
                cmd.Parameters.Add(new SqlParameter("@IdMetodoPago", SqlDbType.Int) { Value = (object?)dto.IdMetodoPago ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@FechaPago", SqlDbType.DateTime) { Value = (object?)dto.FechaPago ?? DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Usuario", SqlDbType.VarChar, 50) { Value = dto.Usuario });

                var pNewPago = new SqlParameter("@NewPagoId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(pNewPago);

                if (conn.State != ConnectionState.Open) conn.Open();
                cmd.ExecuteNonQuery();

                var newPagoId = (int)(pNewPago.Value ?? 0);
                return newPagoId;
            }
            catch (Exception ex)
            {
                _errorLog.RegistrarError(ex);
                throw;
            }
        }

        public IEnumerable<VTAModVentaCobranzaCuotaDTO> ObtenerCuotasPorPlan(int idPlan)
        {
            return _unitOfWork.CobranzaCuotaRepository.Query()
                     .Where(c => c.IdCobranzaPlan == idPlan)
                     .OrderBy(c => c.Numero)
                     .Select(c => new VTAModVentaCobranzaCuotaDTO
                     {
                         Id = c.Id,
                         IdCobranzaPlan = c.IdCobranzaPlan,
                         Numero = c.Numero,
                         FechaVencimiento = c.FechaVencimiento,
                         MontoProgramado = c.MontoProgramado,
                         MontoPagado = c.MontoPagado
                     }).ToList();
        }

        public VTAModVentaCobranzaPlanConCuotasDTO? ObtenerPlanPorOportunidad(int idOportunidad)
        {
            try
            {
                var result = new VTAModVentaCobranzaPlanConCuotasDTO();

                using var conn = _context.Database.GetDbConnection();
                using var cmd = conn.CreateCommand();

                cmd.CommandText = "adm.SP_ObtenerPlanYCuotasPorOportunidad";
                cmd.CommandType = CommandType.StoredProcedure;

                var p_IdOport = new SqlParameter("@IdOportunidad", SqlDbType.Int) { Value = idOportunidad };
                cmd.Parameters.Add(p_IdOport);

                if (conn.State != ConnectionState.Open) conn.Open();

                using var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var planDto = new VTAModVentaCobranzaPlanDTO
                    {
                        Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                        IdOportunidad = reader.IsDBNull(reader.GetOrdinal("IdOportunidad")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdOportunidad")),
                        Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0m : reader.GetDecimal(reader.GetOrdinal("Total")),
                        NumCuotas = reader.IsDBNull(reader.GetOrdinal("NumCuotas")) ? 0 : reader.GetInt32(reader.GetOrdinal("NumCuotas")),
                        FechaInicio = reader.IsDBNull(reader.GetOrdinal("FechaInicio")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                        FrecuenciaDias = reader.IsDBNull(reader.GetOrdinal("FrecuenciaDias")) ? 0 : reader.GetInt32(reader.GetOrdinal("FrecuenciaDias"))
                    };

                    result.Plan = planDto;
                }
                else
                {
                    result.Plan = null;
                }

                // Result set 2: Cuotas
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var cuota = new VTAModVentaCobranzaCuotaDTO
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                            IdCobranzaPlan = reader.IsDBNull(reader.GetOrdinal("IdCobranzaPlan")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdCobranzaPlan")),
                            Numero = reader.IsDBNull(reader.GetOrdinal("Numero")) ? 0 : reader.GetInt32(reader.GetOrdinal("Numero")),
                            FechaVencimiento = reader.IsDBNull(reader.GetOrdinal("FechaVencimiento")) ? default(DateTime) : reader.GetDateTime(reader.GetOrdinal("FechaVencimiento")),
                            MontoProgramado = reader.IsDBNull(reader.GetOrdinal("MontoProgramado")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MontoProgramado")),
                            MontoPagado = reader.IsDBNull(reader.GetOrdinal("MontoPagado")) ? 0m : reader.GetDecimal(reader.GetOrdinal("MontoPagado"))
                        };

                        result.Cuotas.Add(cuota);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _errorLog.RegistrarError(ex);
                throw;
            }
        }
    }
}
