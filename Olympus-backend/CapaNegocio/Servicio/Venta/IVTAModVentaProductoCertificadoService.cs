using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaProductoCertificadoService
    {
        VTAModVentaProductoCertificadoDTORPT ObtenerTodas();
        VTAModVentaProductoCertificadoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoCertificadoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoCertificadoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
