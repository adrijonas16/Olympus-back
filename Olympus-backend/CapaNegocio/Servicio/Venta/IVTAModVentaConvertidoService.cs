using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaConvertidoService
    {
        VTAModVentaConvertidoDTORPT ObtenerTodas();
        VTAModVentaConvertidoDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaConvertidoDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaConvertidoDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
    }
}
