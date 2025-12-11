using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaBeneficioService
    {
        VTAModVentaBeneficioDTORPT ObtenerTodas();
        VTAModVentaBeneficioDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaBeneficioDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaBeneficioDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
