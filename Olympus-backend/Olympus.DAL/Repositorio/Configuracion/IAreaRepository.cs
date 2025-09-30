using Modelos.DTO.Configuracion;
using Modelos.Entidades;

namespace CapaDatos.Repositorio.Configuracion
{ 
    public interface IAreaRepository
    {
        bool Insertar(Area modelo);
        List<CFGModPermisosTAreaDTO> ObtenerTodas();
    }
}
