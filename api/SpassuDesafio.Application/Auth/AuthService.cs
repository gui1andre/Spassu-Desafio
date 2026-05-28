using SpassuDesafio.Application.Auth.DTOs;
using SpassuDesafio.Application.Auth.Interfaces;

namespace SpassuDesafio.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IIdentityManager _identityManager;
    private readonly ITokenService _tokenService;

    public AuthService(IIdentityManager identityManager, ITokenService tokenService)
    {
        _identityManager = identityManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request)
    {
        var isValid = await _identityManager.CheckPasswordAsync(request.Email, request.Password);
        
        if (!isValid)
        {
            throw new UnauthorizedAccessException("Usuário ou senha inválidos.");
        }

        return await GenerateAuthResponseAsync(request.Email);
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
    {
        await _identityManager.CreateUserAsync(request.Email, request.Password);
        
        return await GenerateAuthResponseAsync(request.Email);
    }

    public async Task<AuthResponseDTO> RefreshTokenAsync(string email, string refreshToken)
    {
        var isValid = await _identityManager.ValidateRefreshTokenAsync(email, refreshToken);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Refresh Token inválido ou expirado.");
        }

        return await GenerateAuthResponseAsync(email);
    }

    private async Task<AuthResponseDTO> GenerateAuthResponseAsync(string email)
    {
        var roles = await _identityManager.GetUserRolesAsync(email);
        var response = _tokenService.GenerateToken(email, roles);

        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(1);

        await _identityManager.SetRefreshTokenAsync(email, refreshToken, refreshTokenExpiration);

        response.RefreshToken = refreshToken;
        response.RefreshTokenExpiration = refreshTokenExpiration;

        return response;
    }
}
