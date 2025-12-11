using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaOcurrenciaService
    {
        VTAModVentaOcurrenciaDTORPT ObtenerTodas();
        VTAModVentaOcurrenciaDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaOcurrenciaDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaOcurrenciaDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
