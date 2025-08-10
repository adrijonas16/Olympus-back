using CapaDatos.Repositorios;
using CapaNegocio.Servicios;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modelos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class UsuariosService : IUsuariosService
{
    private readonly IUsuariosRepository _usuariosRepository;
    private readonly IConfiguration _config;

    public UsuariosService(IUsuariosRepository usuariosRepository, IConfiguration config)
    {
        _usuariosRepository = usuariosRepository;
        _config = config;
    }

    public async Task<string?> Autenticar(string correo, string password)
    {
        var usuario = await _usuariosRepository.ObtenerPorCorreo(correo);

        if (usuario == null || usuario.Password != password) // Aquí deberías usar hash, no texto plano
            return null;

        return GenerarToken(usuario);
    }

    private string GenerarToken(Usuario usuario)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Correo),
            new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
