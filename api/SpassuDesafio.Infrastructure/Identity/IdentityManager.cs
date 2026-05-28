using Microsoft.AspNetCore.Identity;
using SpassuDesafio.Application.Auth.Interfaces;

namespace SpassuDesafio.Infrastructure.Identity;

public class IdentityManager : IIdentityManager
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        return result.Succeeded;
    }

    public async Task CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(" ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException(errors);
        }
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Enumerable.Empty<string>();

        return await _userManager.GetRolesAsync(user);
    }

    public async Task SetRefreshTokenAsync(string email, string refreshToken, DateTime expiration)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return;

        await _userManager.RemoveAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshToken");
        await _userManager.SetAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshToken", refreshToken);

        await _userManager.RemoveAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshTokenExpiration");
        await _userManager.SetAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshTokenExpiration", expiration.ToString("o"));
    }

    public async Task<bool> ValidateRefreshTokenAsync(string email, string refreshToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var storedToken = await _userManager.GetAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshToken");
        var storedExpiration = await _userManager.GetAuthenticationTokenAsync(user, "SpassuDesafio", "RefreshTokenExpiration");

        if (storedToken != refreshToken || string.IsNullOrEmpty(storedExpiration))
        {
            return false;
        }

        if (DateTime.TryParse(storedExpiration, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expirationDate))
        {
            if (expirationDate < DateTime.UtcNow)
            {
                return false;
            }
            return true;
        }

        return false;
    }
}
