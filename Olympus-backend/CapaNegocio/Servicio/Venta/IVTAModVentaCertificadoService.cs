using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaCertificadoService
    {
        VTAModVentaCertificadoDTORPT ObtenerTodas();
        VTAModVentaCertificadoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaCertificadoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaCertificadoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
