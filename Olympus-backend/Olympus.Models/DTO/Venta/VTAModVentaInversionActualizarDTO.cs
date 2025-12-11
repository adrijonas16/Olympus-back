using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaInversionActualizarDTO
    {
        public int IdProducto { get; set; }
        public int IdOportunidad { get; set; }
        public decimal DescuentoPorcentaje { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        public int? Id { get; set; }
        public decimal? CostoTotal { get; set; }
        public string Moneda { get; set; } = string.Empty;
        public decimal? CostoOfrecido { get; set; }
        public decimal? DescuentoAplicado { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsuarioModificacionSalida { get; set; } = string.Empty;
    }
}
