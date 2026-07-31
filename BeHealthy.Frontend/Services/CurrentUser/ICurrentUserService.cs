using BeHealthy.Shared;

namespace BeHealthy.Frontend.Services.CurrentUser;

/// <summary>
/// Temporary stand-in for authentication/authorization, which is intentionally out of scope
/// for the initial WASM migration. Pages branch on the current user's role/id; this stub
/// supplies a fixed Admin identity so that logic keeps working until real auth is wired in.
/// </summary>
public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    UserRole? Role { get; }
    bool IsAdmin { get; }
}
