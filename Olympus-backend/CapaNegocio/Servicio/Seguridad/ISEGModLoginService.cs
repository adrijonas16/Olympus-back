using Modelos.DTO.Seguridad;

namespace CapaNegocio.Servicio.Configuracion
{
    public interface ISEGModLoginService
    {
        LoginResponseDTO Autenticar(string correo, string password, string ip);
    }
}
