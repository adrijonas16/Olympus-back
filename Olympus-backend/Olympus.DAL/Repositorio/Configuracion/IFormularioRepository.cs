using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{
    public interface IFormularioRepository
    {
        List<CFGModPermisosTFormularioDTO> ObtenerFormularioPorModulo(int idModulo);
    }
}
