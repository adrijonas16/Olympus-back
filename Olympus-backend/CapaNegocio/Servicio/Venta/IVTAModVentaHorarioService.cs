using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaHorarioService
    {
        VTAModVentaHorarioDTORPT ObtenerTodas();
        VTAModVentaHorarioDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaHorarioDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaHorarioDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
