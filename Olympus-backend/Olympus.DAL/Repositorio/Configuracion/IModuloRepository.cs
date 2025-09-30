using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{ 
    public interface IModuloRepository
    {
        List<CFGModPermisosTModuloDTO> ObtenerPorAreas(List<int> idsAreas);
    }
}
