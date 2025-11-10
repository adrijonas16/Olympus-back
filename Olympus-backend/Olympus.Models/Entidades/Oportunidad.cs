using System;
using System.Collections.Generic;

namespace Modelos.Entidades
{
    public class Oportunidad
    {
        public int Id { get; set; }
        public int IdPersona { get; set; }
        public int? IdProducto { get; set; }
        public string? CodigoLanzamiento { get; set; }
        public string? Origen { get; set; }
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        

        // Navegaciones
        public Persona? Persona { get; set; }
        public Producto? Producto { get; set; }
        public List<ControlOportunidad> ControlOportunidades { get; set; } = new List<ControlOportunidad>();
        public List<HistorialEstado> HistorialEstado { get; set; } = new List<HistorialEstado>();
        public List<HistorialInteraccion> HistorialInteracciones { get; set; } = new List<HistorialInteraccion>();
        public List<Inversion> Inversion { get; set; } = new List<Inversion>();
    }
}
