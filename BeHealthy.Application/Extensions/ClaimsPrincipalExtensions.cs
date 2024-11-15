using System.Security.Claims;

namespace BeHealthy.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal) => principal.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    public static string GetUserRole(this ClaimsPrincipal principal) => principal.FindFirst(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;
}
