using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaPaisService
    {
        VTAModVentaTPaisDTORPT ObtenerTodas();
        VTAModVentaTPaisDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTPaisDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTPaisDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
