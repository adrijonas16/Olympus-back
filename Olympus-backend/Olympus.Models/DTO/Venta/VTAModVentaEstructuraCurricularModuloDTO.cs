using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaEstructuraCurricularModuloDTO
    {
        public int Id { get; set; }
        public int IdEstructuraCurricular { get; set; }
        public int IdModulo { get; set; }
        public VTAModVentaModuloDTO? Modulo { get; set; }
        public int? Orden { get; set; }
        public int? Sesiones { get; set; }
        public int? DuracionHoras { get; set; }
        public string? Observaciones { get; set; }

        // Docente asignado
        public int? IdDocente { get; set; }
        public int? IdPersonaDocente { get; set; }
        public string? DocenteNombre { get; set; }
        public string DocenteLogros { get; set; } = string.Empty;
        public string ModuloNombre { get; set; } = string.Empty;
        public VTAModVentaHorarioDTO? Horarios { get; set; }
    }
}
