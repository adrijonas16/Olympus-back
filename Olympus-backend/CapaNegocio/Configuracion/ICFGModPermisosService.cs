using Modelos.DTO.Configuracion;
using Modelos.Entidades;
using System.Collections.Generic;

namespace CapaNegocio.Servicio.Configuracion
{
    public interface ICFGModPermisosService
    {
        CFGModPermisosTAreaDTORPT ObtenerTodas();
        CFGModPermisosTModuloDTORPT ObtenerPorAreas(List<int> idsAreas);
        CFGModPermisosTFormularioDTORPT ObtenerFormularioPorModulo(int idModulo);
    }
}
