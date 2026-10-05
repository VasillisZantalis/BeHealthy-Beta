namespace BeHealthy.Application.Services.Interfaces;

public interface IUserService
{
    Task<Dictionary<string, int>> GetUsersInRolesCount(CancellationToken cancellationToken = default);
    Task<ServiceResponse> CreateApplicationUser(ApplicationUser applicationUser, string password, CancellationToken cancellationToken = default);
    Task<bool> IsEmailInUseAsync(string email, CancellationToken cancellationToken = default);
    Task<ServiceResponse> AddUserToRoleAsync(ApplicationUser user, UserRole role, CancellationToken cancellationToken = default);
    Task<ServiceResponse> RemoveUserFromRoleAsync(ApplicationUser user, UserRole role, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<ServiceResponse> DeleteUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateUserAsync(ApplicationUser applicationUser, CancellationToken cancellationToken = default);
    Task<ServiceResponse> CreateAdminAsync(ApplicationUser applicationUser, string password, CancellationToken cancellationToken = default);
}
