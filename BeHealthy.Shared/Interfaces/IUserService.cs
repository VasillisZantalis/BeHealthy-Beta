namespace BeHealthy.Shared.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(string id);
}
