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

        var roles = await _identityManager.GetUserRolesAsync(request.Email);
        return _tokenService.GenerateToken(request.Email, roles);
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request)
    {
        await _identityManager.CreateUserAsync(request.Email, request.Password);
        
        var roles = await _identityManager.GetUserRolesAsync(request.Email);
        return _tokenService.GenerateToken(request.Email, roles);
    }
}
