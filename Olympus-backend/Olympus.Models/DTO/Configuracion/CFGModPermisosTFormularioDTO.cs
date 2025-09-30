namespace Modelos.DTO.Configuracion
{
    public class CFGModPermisosTFormularioDTO
    {
        public int Id { get; set; }
        public int IdArea { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

    public class CFGModPermisosTFormularioDTORPT : CFGRespuestaGenericaDTO
    {
        public List<CFGModPermisosTFormularioDTO> Formulario { get; set; }
    }
}
