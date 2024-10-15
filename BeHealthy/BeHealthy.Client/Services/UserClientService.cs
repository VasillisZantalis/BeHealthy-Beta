using BeHealthy.Shared.Interfaces;

namespace BeHealthy.Client.Services;

public class UserClientService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task DeleteUserAsync(string id)
    {
        await _httpClient.DeleteAsync($"api/users/{id}");
    }
}
