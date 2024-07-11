using BeHealthy.Client.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Room;
using System.Net.Http.Json;

namespace BeHealthy.Client.Services;

public class RoomService : IRoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<IEnumerable<RoomDto>>("api/rooms");
        return rooms ?? new List<RoomDto>();
    }

    public async Task<RoomDto> GetRoomByIdAsync(int id)
    {
        var rooms = await _httpClient.GetFromJsonAsync<RoomDto>($"api/rooms/{id}");
        return rooms ?? new RoomDto();
    }

    public async Task AddRoomAsync(RoomForCreationDto roomDto)
    {
        var response = await _httpClient.PostAsJsonAsync("api/rooms", roomDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateRoomAsync(int id, RoomForUpdateDto roomDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/rooms/{id}", roomDto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRoomAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/rooms/{id}");
        response.EnsureSuccessStatusCode();
    }
}
