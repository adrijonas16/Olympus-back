using Modelos.DTO.Configuracion;
using Modelos.DTO.Venta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Servicio.Venta
{
    public interface IVTAModVentaPotencialClienteService
    {
        VTAModVentaPotencialClienteDTORPT ObtenerTodas();
        VTAModVentaPotencialClienteDTO ObtenerPorId(int id);
        CFGRespuestaGenericaDTO Insertar(VTAModVentaPotencialClienteDTO dto);
        CFGRespuestaGenericaDTO Actualizar(VTAModVentaPotencialClienteDTO dto);
        CFGRespuestaGenericaDTO Eliminar(int id);
        CFGRespuestaGenericaDTO ImportarProcesadoLinkedin(DateTime? fechaInicio, DateTime? fechaFin);
    }
}
