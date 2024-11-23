using BeHealthy.Domain;
using System.Security.Claims;

namespace BeHealthy.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal) => principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    public static string GetUserRole(this ClaimsPrincipal principal) => principal.FindFirst(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

    public static UserRole? GetUserRoleEnum(this ClaimsPrincipal principal)
    {
        var role = principal.GetUserRole();
        if (!string.IsNullOrEmpty(role) && Enum.TryParse(typeof(UserRole), role, out var roleEnum))
        {
            return (UserRole)roleEnum;
        }

        return null;
    }
}
