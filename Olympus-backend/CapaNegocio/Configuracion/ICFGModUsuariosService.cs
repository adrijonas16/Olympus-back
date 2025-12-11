using Modelos.DTO.Configuracion;

namespace CapaNegocio.Configuracion
{
    public interface ICFGModUsuariosService
    {
        CFGModUsuariosTUsuarioDTORPT RegistrarUsuarioYPersona(CFGModUsuariosTUsuarioDTO modelo);
        CFGModUsuariosListadoDTORPT ListarConUsuario();
        CFGModUsuariosTUsuarioDTORPT EditarUsuarioYPersona(CFGModUsuariosTUsuarioDTO modelo, int idUsuario);
        CFGModUsuariosTUsuarioDTORPT EliminarUsuarioYPersona(int idUsuario);
        CFGModUsuariosTRolDTORPT ObtenerRoles();
        CFGModUsuarioPorRolDTORPT ObtenerUsuariosPorRol(int idRol);

    }
}
