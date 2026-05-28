using Microsoft.AspNetCore.Mvc;
using SpassuDesafio.Application.Auth.DTOs;
using SpassuDesafio.Application.Auth.Interfaces;

namespace SpassuDesafio.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        var result = await _authService.RegisterAsync(request);
        SetRefreshTokenCookie(result);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _authService.LoginAsync(request);
        SetRefreshTokenCookie(result);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromQuery] string email)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized("Refresh Token não encontrado.");

        var result = await _authService.RefreshTokenAsync(email, refreshToken);
        SetRefreshTokenCookie(result);
        return Ok(result);
    }

    private void SetRefreshTokenCookie(AuthResponseDTO response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = response.RefreshTokenExpiration
        };

        Response.Cookies.Append("refreshToken", response.RefreshToken!, cookieOptions);

        response.RefreshToken = null;
        response.RefreshTokenExpiration = null;
    }
}
