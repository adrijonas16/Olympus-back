﻿using CapaDatos.Repositorio.UnitOfWork;
using CapaNegocio.Configuracion;
using CapaNegocio.Servicio.Configuracion;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Modelos.DTO.Seguridad;
using Modelos.Entidades;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class SEGModLoginService : ISEGModLoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public SEGModLoginService(IUnitOfWork unitOfWork, IConfiguration config)
    {
        _unitOfWork = unitOfWork;
        _config = config;
    }

    /// <summary>
    /// Método de autenticación.
    /// </summary>
    /// <UsuarioCreacion>Adriana Chipana</UsuarioCreacion>
    /// <FechaCreacion>2025-08-27</FechaCreacion>
    /// <UsuarioModificacion>Adriana Chipana</UsuarioModificacion>
    /// <FechaModificacion>2025-08-28</FechaModificacion>
    public LoginResponseDTO Autenticar(string correo, string password)
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

            Token = GenerarToken(usuario);

            respuesta.Codigo = SR._C_SIN_ERROR;
            respuesta.Mensaje = string.Empty;
            respuesta.Token = Token;
        }
        catch (Exception ex)
        {
            respuesta.Codigo = SR._C_ERROR_CRITICO;
            respuesta.Mensaje = ex.Message;
        }
        return respuesta;
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
