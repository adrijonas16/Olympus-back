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

        public VTAModVentaCobranzaPlanDTO? ObtenerPlanPorOportunidad(int idOportunidad)
        {
            var plan = _unitOfWork.CobranzaPlanRepository.Query().FirstOrDefault(p => p.IdOportunidad == idOportunidad);
            if (plan == null) return null;
            return new VTAModVentaCobranzaPlanDTO
            {
                Id = plan.Id,
                IdOportunidad = plan.IdOportunidad,
                Total = plan.Total,
                NumCuotas = plan.NumCuotas,
                FechaInicio = plan.FechaInicio,
                FrecuenciaDias = plan.FrecuenciaDias
            };
        }
    }
}
