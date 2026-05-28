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
}
