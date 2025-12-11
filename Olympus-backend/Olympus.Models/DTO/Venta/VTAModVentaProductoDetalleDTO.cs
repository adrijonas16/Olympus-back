using Modelos.DTO.Configuracion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaProductoDetalleRPT : CFGRespuestaGenericaDTO
    {
        public VTAModVentaProductoDTO Producto { get; set; } = new VTAModVentaProductoDTO();
        public List<VTAModVentaHorarioDTO> Horarios { get; set; } = new();
        public List<VTAModVentaInversionDTO> Inversiones { get; set; } = new();
        public List<VTAModVentaEstructuraCurricularDTO> Estructuras { get; set; } = new();
        public List<VTAModVentaEstructuraCurricularModuloDTO> EstructuraModulos { get; set; } = new();
        public List<VTAModVentaEstructuraCurricularModuloDTO> DocentesPorModulo { get; set; } = new();
        public List<VTAModVentaProductoCertificadoDTO> ProductoCertificados { get; set; } = new();
        public List<VTAModVentaMetodoPagoProductoDTO> MetodosPago { get; set; } = new();
        public List<VTAModVentaBeneficioDTO> Beneficios { get; set; } = new();
    }
}
