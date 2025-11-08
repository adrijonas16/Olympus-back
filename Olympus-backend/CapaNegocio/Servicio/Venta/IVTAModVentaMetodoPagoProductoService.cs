using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaMetodoPagoProductoService
    {
        VTAModVentaMetodoPagoProductoDTORPT ObtenerTodas();
        VTAModVentaMetodoPagoProductoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaMetodoPagoProductoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaMetodoPagoProductoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
