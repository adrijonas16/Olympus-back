using CapaDatos.DataContext;
using Dapper;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class ModuloRepository : IModuloRepository
    {
        private readonly OlympusContext _context;
        public ModuloRepository(OlympusContext context)
        {
            _context = context;
        }
        public List<CFGModPermisosTModuloDTO> ObtenerPorAreas(List<int> idsAreas)
        {
            const string sql = @"
                SELECT m.Id, m.Nombre, m.Activo, m.IdArea
                FROM Modulo m
                WHERE m.IdArea IN @IdsAreas;";

            using (var connection = _context.CreateConnection())
            {
                return connection.Query<CFGModPermisosTModuloDTO>(
                    sql,
                    new { IdsAreas = idsAreas }
                ).ToList();
            }
        }


    }
}
