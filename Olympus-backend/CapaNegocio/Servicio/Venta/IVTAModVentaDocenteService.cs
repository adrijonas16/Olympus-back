using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaDocenteService
    {
        VTAModVentaDocenteDTORPT ObtenerTodas();
        VTAModVentaDocenteDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaDocenteDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaDocenteDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
