using CapaDatos.Repositorio.UnitOfWork;

public class TokenService
{
    private readonly IUnitOfWork _unitOfWork;

    public TokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public bool ValidarYRenovarToken(string token, out string tokenRenovado)
    {
        tokenRenovado = null;
        var userToken = _unitOfWork.UserTokenRepository.ObtenerPorToken(token);

        if (userToken == null || userToken.Expiration < DateTime.Now || userToken.IsRevoked)
            return false;

        // Renovar expiración y fecha de modificación
        userToken.Expiration = DateTime.Now.AddHours(5);
        userToken.FechaModificacion = DateTime.Now;
        _unitOfWork.UserTokenRepository.Actualizar(userToken);
        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        tokenRenovado = userToken.Token;
        return true;
    }

    /// <summary>
    /// Verifica y renueva el token. Devuelve el token renovado si es válido, o null si no lo es.
    /// </summary>
    public string VerificarYRenovarToken(string token)
    {
        var userToken = _unitOfWork.UserTokenRepository.ObtenerPorToken(token);

        if (userToken == null || userToken.Expiration < DateTime.Now || userToken.IsRevoked)
            return null;

        // Renovar expiración y fecha de modificación
        userToken.Expiration = DateTime.Now.AddHours(5);
        userToken.FechaModificacion = DateTime.Now;
        _unitOfWork.UserTokenRepository.Actualizar(userToken);
        _unitOfWork.SaveChangesAsync().GetAwaiter().GetResult();

        return userToken.Token;
    }
}