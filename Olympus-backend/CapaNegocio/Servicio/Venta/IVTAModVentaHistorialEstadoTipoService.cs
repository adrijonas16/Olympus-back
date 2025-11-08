using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaHistorialEstadoTipoService
    {
        VTAModVentaHistorialEstadoTipoDTORPT ObtenerTodas();
        VTAModVentaHistorialEstadoTipoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaHistorialEstadoTipoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaHistorialEstadoTipoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);

    }
}
