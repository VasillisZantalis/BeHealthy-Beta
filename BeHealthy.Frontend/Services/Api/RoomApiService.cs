using BeHealthy.Frontend.Services.Interfaces;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Frontend.Services.Api;

public class RoomApiService : ApiClientBase, IRoomService
{
    public RoomApiService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

    public async Task<IEnumerable<RoomResponse>> GetAllRoomsAsync()
        => await GetListAsync<RoomResponse>("rooms");

    public async Task<RoomResponse?> GetRoomByIdAsync(int id)
        => await GetAsync<RoomResponse>($"rooms/{id}");

    public async Task AddRoomAsync(RoomCreateRequest roomDto)
        => await PostAsync("rooms", roomDto);

    public async Task UpdateRoomAsync(RoomUpdateRequest roomDto)
        => await PutAsync("rooms", roomDto);

    public async Task DeleteRoomAsync(int id)
        => await DeleteAsync($"rooms/{id}");
}
