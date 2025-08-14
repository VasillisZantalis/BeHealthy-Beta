namespace BeHealthy.Application.Services.Interfaces;

public interface IUserService
{
    Task<Dictionary<string, int>> GetUsersInRolesCount();
    Task<ServiceResponse> CreateApplicationUser(ApplicationUser applicationUser, string password, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddUserToRoleAsync(ApplicationUser user, UserRole role);
    Task<ServiceResponse> RemoveUserFromRoleAsync(ApplicationUser user, UserRole role);
    Task<ApplicationUser?> GetUserByIdAsync(string userId);
    Task<ServiceResponse> DeleteUserAsync(ApplicationUser applicationUser);
    Task<ServiceResponse> UpdateUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken = default);
}
