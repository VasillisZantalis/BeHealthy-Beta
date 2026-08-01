using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Frontend.Services.Api;

public class RoomApiService : ApiClientBase, IRoomService
{
    public RoomApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
        => await GetListAsync<RoomDto>("rooms");

    public async Task<RoomDto?> GetRoomByIdAsync(int id)
        => await GetAsync<RoomDto>($"rooms/{id}");

    public async Task AddRoomAsync(RoomCreateDto roomDto)
        => await PostAsync("rooms", roomDto);

    public async Task UpdateRoomAsync(RoomUpdateDto roomDto)
        => await PutAsync("rooms", roomDto);

    public async Task DeleteRoomAsync(int id)
        => await DeleteAsync($"rooms/{id}");
}
