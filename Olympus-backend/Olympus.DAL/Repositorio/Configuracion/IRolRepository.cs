using CapaNegocio.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public interface IRolRepository
    {
        bool Insertar(Rol modelo);
        List<CFGModUsuariosTRolDTO> ObtenerTodas();
    }
}
