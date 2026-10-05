using BeHealthy.Shared;

namespace BeHealthy.Front.Services.CurrentUser;

/// <summary>
/// Exposes the signed-in user's identity for the lifetime of the current circuit, backed by the
/// authentication cookie issued at login.
/// </summary>
public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? UserId { get; }
    string? UserName { get; }
    UserRole? Role { get; }
    bool IsAdmin { get; }

    /// <summary>The JWT to attach as a Bearer token when calling BeHealthy.API.</summary>
    string? Token { get; }
}
