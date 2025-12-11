using Modelos.DTO.Configuracion;
using Modelos.DTO.Seguridad;
using Modelos.DTO.Venta;

namespace CapaNegocio.Servicio.Configuracion
{
    public interface ISEGModLoginService
    {
        LoginResponseDTO Autenticar(string correo, string password, string ip);
        CFGRespuestaGenericaDTO ObtenerPermisosDeOportunidad(int IdOportunidad, int IdUsuario, int IdRol);

    }
}
