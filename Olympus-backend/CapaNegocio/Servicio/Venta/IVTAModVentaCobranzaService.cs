using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaCobranzaService
    {
        VTAModVentaCobranzaDTORPT ObtenerTodas();
        VTAModVentaCobranzaDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaCobranzaDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaCobranzaDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
