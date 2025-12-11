using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaImportarLinkedinSkippedDTO
    {
        public int Id { get; set; }
        public string FormName { get; set; } = string.Empty;
        public string Motivo { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
    }
}
