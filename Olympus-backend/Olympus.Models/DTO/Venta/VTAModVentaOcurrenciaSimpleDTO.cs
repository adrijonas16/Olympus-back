using Modelos.DTO.Configuracion;

namespace Modelos.DTO.Venta
{
    public class VTAModVentaOcurrenciaSimpleDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int? IdEstado { get; set; }
        public bool Allowed { get; set; }
    }

    public class VTAModVentaOcurrenciasPermitidasDTORPT : CFGRespuestaGenericaDTO
    {
        public List<VTAModVentaOcurrenciaSimpleDTO> Ocurrencias { get; set; } = new List<VTAModVentaOcurrenciaSimpleDTO>();
    }

    public class VTAModVentaAsignarAsesorDTO
    {
        public List<int> IdOportunidades { get; set; } = new List<int>();
        public int IdAsesor { get; set; }
        public string? UsuarioModificacion { get; set; }
    }

}
