namespace Modelos.DTO.Configuracion
{
    public class CFGModPermisosTModuloDTO
    {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CFGModPermisosTModuloDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModPermisosTModuloDTO> Modulo { get; set; }
    }
}
