using SpassuDesafio.Application.Auth.DTOs;

namespace SpassuDesafio.Application.Auth.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> LoginAsync(LoginRequestDTO request);
    Task<AuthResponseDTO> RegisterAsync(RegisterRequestDTO request);
}
