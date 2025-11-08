using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaInversionDescuentoService
    {
        VTAModVentaInversionDescuentoDTORPT ObtenerTodas();
        VTAModVentaInversionDescuentoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaInversionDescuentoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaInversionDescuentoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
