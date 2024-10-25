namespace BeHealthy.Services.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(string id);
}
