using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? CodigoLanzamiento { get; set; } = string.Empty;
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime? FechaPresentacion { get; set; }
        public string? DatosImportantes { get; set; } = string.Empty;
        public bool Estado { get; set; } = true;
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
        public string? UsuarioModificacion { get; set; } = string.Empty;

        // Navegaciones
        public List<Horario> Horarios { get; set; } = new List<Horario>();
        public List<Inversion> Inversiones { get; set; } = new List<Inversion>();
        public List<MetodoPagoProducto> MetodoPagoProductos { get; set; } = new List<MetodoPagoProducto>();
        public List<ProductoCertificado> ProductoCertificados { get; set; } = new List<ProductoCertificado>();
        public List<Beneficio> Beneficios { get; set; } = new List<Beneficio>();
        public List<Cobranza> Cobranzas { get; set; } = new List<Cobranza>();
        public List<Convertido> Convertidos { get; set; } = new List<Convertido>();
        public List<VentaCruzada> VentaCruzadaOrigen { get; set; } = new List<VentaCruzada>();
        public List<VentaCruzada> VentaCruzadaDestino { get; set; } = new List<VentaCruzada>();
        public List<Corporativo> Corporativos { get; set; } = new List<Corporativo>();
        public List<Oportunidad> Oportunidades { get; set; } = new List<Oportunidad>();
    }
}
