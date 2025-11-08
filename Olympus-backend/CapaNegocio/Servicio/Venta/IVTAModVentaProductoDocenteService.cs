using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaProductoDocenteService
    {
        VTAModVentaProductoDocenteDTORPT ObtenerTodas();
        VTAModVentaProductoDocenteDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaProductoDocenteDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaProductoDocenteDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
