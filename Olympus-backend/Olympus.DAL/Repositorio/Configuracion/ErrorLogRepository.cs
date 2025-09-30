using CapaDatos.DataContext;
using Dapper;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly OlympusContext _context;
        public ErrorLogRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Insertar(ErrorLog modelo)
        {
            const string sql = @"
                INSERT INTO ErrorLog
                (FechaHora, Origen, Mensaje, Traza)
                VALUES (@FechaHora, @Origen, @Mensaje, @Traza);";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Execute(sql, modelo);
                return result > 0;
            }
        }

    }
}
