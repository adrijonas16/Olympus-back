using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Pais
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int PrefijoCelularPais { get; set; }
        public int DigitoMaximo { get; set; }
        public int DigitoMinimo { get; set; }
        public bool Estado { get; set; }
        public int? IdMigracion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacion { get; set; } = string.Empty;
        public DateTime FechaModificacion { get; set; }
        public string UsuarioModificacion { get; set; } = string.Empty;
        public byte[]? RowVersion { get; set; }

        // navegación inversa
        public List<Persona> Personas { get; set; } = new List<Persona>();
        public List<Asesor> Asesores { get; set; } = new List<Asesor>();
    }
}
