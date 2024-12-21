using BeHealthy.Application.Dtos.User;

namespace BeHealthy.Application.Services.Interfaces;

public interface IUserService
{
    Task<Dictionary<string, int>> GetUsersInRolesCount();
}
