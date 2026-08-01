using BeHealthy.Shared;

namespace BeHealthy.Frontend.Services.CurrentUser;

/// <inheritdoc />
public class CurrentUserService : ICurrentUserService
{
    // TODO: replace with real authentication state once auth is reintroduced.
    public string? UserId => "00000000-0000-0000-0000-000000000000";
    public string? UserName => "Administrator";
    public UserRole? Role => UserRole.Admin;
    public bool IsAdmin => Role == UserRole.Admin;
}
