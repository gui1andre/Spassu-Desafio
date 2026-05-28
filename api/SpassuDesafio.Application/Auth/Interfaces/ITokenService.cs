using SpassuDesafio.Application.Auth.DTOs;

namespace SpassuDesafio.Application.Auth.Interfaces;

public interface ITokenService
{
    AuthResponseDTO GenerateToken(string email, IEnumerable<string> roles);
    string GenerateRefreshToken();
}
