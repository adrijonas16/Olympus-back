using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaMetodoPagoService
    {
        VTAModVentaMetodoPagoDTORPT ObtenerTodas();
        VTAModVentaMetodoPagoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaMetodoPagoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaMetodoPagoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
