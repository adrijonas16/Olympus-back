using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaInversionService
    {
        VTAModVentaInversionDTORPT ObtenerTodas();
        VTAModVentaInversionDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaInversionDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaInversionDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
        VTAModVentaInversionActualizarDTO ActualizarCostoOfrecido(VTAModVentaInversionActualizarDTO dto);
    }
}
