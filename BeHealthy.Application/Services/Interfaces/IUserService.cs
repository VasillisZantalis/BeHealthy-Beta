namespace BeHealthy.Application.Services.Interfaces;

public interface IUserService
{
    Task DeleteUserAsync(string id);
}
