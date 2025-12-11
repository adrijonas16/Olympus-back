using CapaDatos.DataContext;
using CapaNegocio.Configuracion;
using Dapper;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class RolRepository : IRolRepository
    {
        private readonly OlympusContext _context;
        public RolRepository(OlympusContext context)
        {
            _context = context;
        }
        public bool Insertar(Rol modelo)
        {
            const string sql = @"
        INSERT INTO Rol
        (NombreRol, FechaCreacion, UsuarioCreacion, FechaModificacion, UsuarioModificacion, Estado)
        VALUES (@Nombre, @FechaCreacion, @UsuarioCreacion, @FechaModificacion, @UsuarioModificacion, @Estado);";

            using (var connection = _context.CreateConnection())
            {
                var result = connection.Execute(sql, modelo);
                return result > 0;
            }
        }

        public List<CFGModUsuariosTRolDTO> ObtenerTodas()
        {
            const string sql = @"
                SELECT Id, NombreRol, Estado
                FROM Rol
                WHERE Estado = 1;";

            using (var connection = _context.CreateConnection())
            {
                return connection.Query<CFGModUsuariosTRolDTO>(sql).ToList();
            }
        }
    }
}
