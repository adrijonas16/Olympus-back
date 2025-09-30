using CapaDatos.DataContext;
using Dapper;
using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public class FormularioRepository : IFormularioRepository
    {
        private readonly OlympusContext _context;
        public FormularioRepository(OlympusContext context)
        {
            _context = context;
        }
        public List<CFGModPermisosTFormularioDTO> ObtenerFormularioPorModulo(int idModulo)
        {
            const string sql = @"
                SELECT m.Id, m.Nombre, m.Activo, m.IdModulo
                FROM Formulario m
                WHERE m.IdModulo = idModulo";

            using (var connection = _context.CreateConnection())
            {
                return connection.Query<CFGModPermisosTFormularioDTO>(
                    sql,
                    new { IdModulo = idModulo }
                ).ToList();
            }
        }


    }
}
