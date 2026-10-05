namespace BeHealthy.Validation.Common;

/// <summary>
/// The password policy. ASP.NET Core Identity is configured from these values (see
/// BeHealthy.Infrastructure DependencyInjection) and the validators mirror them, so the form
/// reports the same rules that Identity enforces when the user is created.
/// </summary>
public static class PasswordPolicy
{
    public const int RequiredLength = 6;
    public const bool RequireDigit = true;
    public const bool RequireLowercase = true;
    public const bool RequireUppercase = true;
    public const bool RequireNonAlphanumeric = true;
}
