using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaCorporativoService
    {
        VTAModVentaCorporativoDTORPT ObtenerTodas();
        VTAModVentaCorporativoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaCorporativoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaCorporativoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
