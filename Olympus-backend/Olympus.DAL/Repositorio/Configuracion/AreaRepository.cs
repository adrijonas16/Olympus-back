using CapaDatos.DataContext;
using Dapper;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class AreaRepository : IAreaRepository
    {
        private readonly OlympusContext _context;
        public AreaRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Insertar(Area modelo)
        {
            const string sql = @"
        INSERT INTO Area
        (Nombre, FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion, Activo)
        VALUES (@Nombre, @FechaCreacion, @UsuarioCreacion, @FechaModificacion, @UsuarioModificacion, @Activo);";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Execute(sql, modelo);
                return result > 0;
            }
        }

        public List<CFGModPermisosTAreaDTO> ObtenerTodas()
        {
            const string sql = @"
                SELECT Id, Nombre, Activo
                FROM Area
                WHERE Activo = 1;";

            using (var connection = _context.CreateConnection())
            {
                return connection.Query<CFGModPermisosTAreaDTO>(sql).ToList();
            }
        }
    }
}
