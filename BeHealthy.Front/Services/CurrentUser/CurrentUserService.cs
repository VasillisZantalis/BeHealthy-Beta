using BeHealthy.Front.Common;
using BeHealthy.Shared;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BeHealthy.Front.Services.CurrentUser;

/// <inheritdoc />
/// <remarks>
/// Scoped to the circuit. The <see cref="ClaimsPrincipal"/> is only reliably available via
/// <see cref="IHttpContextAccessor"/> during the initial request that starts the circuit, so it is
/// captured once here and reused for the circuit's lifetime rather than re-read on every access.
/// </remarks>
public class CurrentUserService : ICurrentUserService
{
    private readonly ClaimsPrincipal _user;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
    }

    public bool IsAuthenticated => _user.Identity?.IsAuthenticated ?? false;
    public string? UserId => _user.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => _user.Identity?.Name;
    public string? Token => _user.FindFirstValue(AuthClaimTypes.ApiToken);

    public UserRole? Role =>
        Enum.TryParse<UserRole>(_user.FindFirstValue(ClaimTypes.Role), out var role) ? role : null;

    public bool IsAdmin => Role == UserRole.Admin;
}
