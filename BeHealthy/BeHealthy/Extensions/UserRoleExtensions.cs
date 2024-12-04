using BeHealthy.Domain;

namespace BeHealthy.Extensions;

public static class UserRoleExtensions
{
    public static string ToRoleString(this UserRole[] roles)
    {
        return string.Join(",", roles.Select(r => r.ToString()));
    }
}
