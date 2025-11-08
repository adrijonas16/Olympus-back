using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaTipoService
    {
        VTAModVentaTipoDTORPT ObtenerTodas();
        VTAModVentaTipoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaTipoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaTipoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
