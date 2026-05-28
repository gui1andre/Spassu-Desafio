using SpassuDesafio.Application.Auth.DTOs;

namespace SpassuDesafio.Application.Auth.Interfaces;

public interface IIdentityManager
{
    Task<bool> CheckPasswordAsync(string email, string password);
    Task CreateUserAsync(string email, string password);
    Task<IEnumerable<string>> GetUserRolesAsync(string email);
    Task SetRefreshTokenAsync(string email, string refreshToken, DateTime expiration);
    Task<bool> ValidateRefreshTokenAsync(string email, string refreshToken);
}
