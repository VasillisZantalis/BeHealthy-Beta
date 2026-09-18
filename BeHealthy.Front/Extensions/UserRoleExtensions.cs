using BeHealthy.Shared;

namespace BeHealthy.Front.Extensions;

public static class UserRoleExtensions
{
    public static string ToRoleString(this UserRole[] roles)
    {
        return string.Join(",", roles.Select(r => r.ToString()));
    }
}
