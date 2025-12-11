using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaProductoService
    {
        VTAModVentaProductoDTORPT ObtenerTodas();
        VTAModVentaProductoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
        VTAModVentaProductoDetalleRPT ObtenerDetallePorOportunidad(int id);
    }
}
