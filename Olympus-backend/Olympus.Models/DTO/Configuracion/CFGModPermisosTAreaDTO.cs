namespace Modelos.DTO.Configuracion
{
    public class CFGModPermisosTAreaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CFGModPermisosTAreaDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModPermisosTAreaDTO> Area { get; set; }
    }
}
