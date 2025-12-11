using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaVentaCruzadaService
    {
        VTAModVentaVentaCruzadaDTORPT ObtenerTodas();
        VTAModVentaVentaCruzadaDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaVentaCruzadaDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaVentaCruzadaDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
