using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class ErrorLog
    {
        public long Id { get; set; }

        public DateTime FechaHora { get; set; }

        public string Origen { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public string? Traza { get; set; }
    }
}
