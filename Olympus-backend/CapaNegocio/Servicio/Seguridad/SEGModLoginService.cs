using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modelos.DTO.Configuracion;
using Modelos.DTO.Seguridad;
using Modelos.DTO.Venta;
using Modelos.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class SEGModLoginService : ISEGModLoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IErrorLogService _errorLogService;

    public SEGModLoginService(IUnitOfWork unitOfWork, IConfiguration config, IErrorLogService errorLogService)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _errorLogService = errorLogService;
    }

    /// <summary>
    /// Método de autenticación.
    /// </summary>
    /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
    /// <FechaCreacion>2025-08-27</FechaCreacion>
    /// <UsuarioModificacion>Adriana Chipana</UsuarioModificacion>
    /// <FechaModificacion>2025-08-28</FechaModificacion>
    public LoginResponseDTO Autenticar(string correo, string password, string ip)
    {
        LoginResponseDTO respuesta = new LoginResponseDTO();
        string Token = string.Empty;
        Usuario usuario = new Usuario();

        try
        {
            usuario = _unitOfWork.UsuarioRepository.ObtenerPorCorreo(correo);
            if (usuario == null || usuario.Password != password)
            {
                respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                respuesta.Mensaje = SR._M_CREDENCIALES_INVALIDAS;

                return respuesta;
            }

            Token = GenerarToken(usuario, ip);

            try
            {
                DateTime expiration = DateTime.Now.AddHours(5);

                var userToken = new UserToken
                {
                    IdUsuario = usuario.Id,
                    IdRol = usuario.IdRol,
                    NombreRol = usuario.Rol.NombreRol,
                    Token = Token,
                    Expiration = expiration,
                    IsRevoked = false,
                    Estado = true,
                    IdMigracion = null,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = usuario.Nombre ?? "SYSTEM",
                    FechaModificacion = DateTime.Now,
                    UsuarioModificacion = usuario.Nombre ?? "SYSTEM",
                    Usuario = usuario
                };

                _unitOfWork.UserTokenRepository.Insertar(userToken);

                _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
            }
            catch (Exception exToken)
            {
                _errorLogService.RegistrarError(exToken);
            }
            respuesta.Codigo = SR._C_SIN_ERROR;
            respuesta.Mensaje = string.Empty;
            respuesta.Token = Token;
        }
        catch (Exception ex)
        {
            _errorLogService.RegistrarError(ex);
            respuesta.Codigo = SR._C_ERROR_CRITICO;
            respuesta.Mensaje = ex.Message;
        }
        return respuesta;
    }

    private string GenerarToken(Usuario usuario, string ip)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim(ClaimTypes.Role, usuario.Rol?.NombreRol ?? "Usuario"),
            new Claim("ip", ip ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(5),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public CFGRespuestaGenericaDTO ObtenerPermisosDeOportunidad(int IdOportunidad, int IdUsuario, int IdRol)
    {
        var respuesta = new CFGRespuestaGenericaDTO();

        try
        {
            // 1) Obtener la oportunidad
            var op = _unitOfWork.OportunidadRepository.ObtenerPorId(IdOportunidad);

            if (op == null)
            {
                respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                respuesta.Mensaje = "La oportunidad no existe.";
                return respuesta;
            }

            // 2) Obtener la persona asociada al usuario (si existe)
            // Asumimos que existe un repositorio o método para obtener Persona por IdUsuario.
            // Si tu repo tiene otro nombre, cámbialo aquí.
            var personaUsuario = _unitOfWork.PersonaRepository.ObtenerPorIdUsuario(IdUsuario);
            // personaUsuario puede ser null si no hay persona ligada al usuario.

            bool tienePermiso = false;

            // 3) Permiso si la persona del usuario es la misma que la persona asociada a la oportunidad
            // Nota: op.IdPersona es la FK que mencionaste (la oportunidad guarda IdPersona)
            if (personaUsuario != null && op.IdPersona == personaUsuario.Id)
            {
                tienePermiso = true;
            }

            // 4) Permiso si el usuario es el asesor asignado a la oportunidad
            // Dependiendo de tu modelo, op.IdAsesor puede contener la IdPersona del asesor.
            // Compararemos con personaUsuario.Id si existe.
            if (!tienePermiso && personaUsuario != null && op.IdPersona.HasValue && op.IdPersona.Value == personaUsuario.Id)
            {
                tienePermiso = true;
            }

            // 5) Permiso por rol (Supervisor, Gerente, Administrador, Desarrollador)
            if (!tienePermiso && (IdRol == 2 || IdRol == 3 || IdRol == 4 || IdRol == 5))
            {
                tienePermiso = true;
            }

            if (!tienePermiso)
            {
                respuesta.Codigo = SR._C_ERROR_CONTROLADO;
                respuesta.Mensaje = "No tienes permiso para ver esta oportunidad.";
                return respuesta;
            }

            respuesta.Codigo = SR._C_SIN_ERROR;
            respuesta.Mensaje = String.Empty;
            return respuesta;
        }
        catch (Exception ex)
        {
            _errorLogService.RegistrarError(ex);
            respuesta.Codigo = SR._C_ERROR_CRITICO;
            respuesta.Mensaje = ex.Message;
        }

        return respuesta;
    }

}
