using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaEstructuraConModulosDTO
    {
        public VTAModVentaEstructuraCurricularDTO Estructura { get; set; } = new VTAModVentaEstructuraCurricularDTO();
        public List<VTAModVentaEstructuraCurricularModuloDTO> Modulos { get; set; } = new List<VTAModVentaEstructuraCurricularModuloDTO>();
    }

    public class VTAModVentaEstructuraConModulosDTORPT : CFGRespuestaGenericaDTO
    {
        public VTAModVentaEstructuraConModulosDTO EstructuraconModulos { get; set; } = new VTAModVentaEstructuraConModulosDTO();
    }
}
