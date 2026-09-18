using BeHealthy.Shared.Dtos.Auth;
using Microsoft.AspNetCore.Identity;

namespace BeHealthy.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(UserManager<ApplicationUser> userManager, IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Username)
            ?? await _userManager.FindByNameAsync(request.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
            return AuthResult.Failed("Invalid username or password.");

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0 || !Enum.TryParse<UserRole>(roles[0], out var role))
            return AuthResult.Failed("This user has no recognized role assigned.");

        var (token, expiresAtUtc) = _jwtTokenService.GenerateToken(user, roles);

        var response = new LoginResponse
        {
            Token = token,
            ExpiresAtUtc = expiresAtUtc,
            User = new UserResponse
            {
                Id = user.Id,
                Username = user.Email ?? user.UserName ?? string.Empty,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = role
            }
        };

        return AuthResult.Ok(response);
    }
}
