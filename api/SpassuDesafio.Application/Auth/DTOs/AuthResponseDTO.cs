namespace SpassuDesafio.Application.Auth.DTOs;

public class AuthResponseDTO
{
    public string? Token { get; set; }
    public DateTime? Expiration { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
}
