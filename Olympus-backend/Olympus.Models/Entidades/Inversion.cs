using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Inversion
    {
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public int IdOportunidad { get; set; }
        public decimal CostoTotal { get; set; } = 0m;
        public string Moneda { get; set; } = string.Empty;
        public decimal? DescuentoPorcentaje { get; set; }
        public decimal? CostoOfrecido { get; set; }
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;

        // Navegación
        public Producto? Producto { get; set; }
        public Oportunidad? Oportunidad { get; set; }
        public List<InversionDescuento> Descuentos { get; set; } = new List<InversionDescuento>();
        public List<Cobranza> Cobranzas { get; set; } = new List<Cobranza>();
        public List<Convertido> Convertidos { get; set; } = new List<Convertido>();
    }
}
