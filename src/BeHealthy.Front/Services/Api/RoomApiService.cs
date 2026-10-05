using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Front.Services.Api;

public class RoomApiService : ApiClientBase, IRoomService
{
    public RoomApiService(IHttpClientFactory httpClientFactory, ICurrentUserService currentUser) : base(httpClientFactory, currentUser) { }

    public async Task<IEnumerable<RoomResponse>> GetAllRoomsAsync()
        => await GetListAsync<RoomResponse>("rooms");

    public async Task<RoomResponse?> GetRoomByIdAsync(int id)
        => await GetAsync<RoomResponse>($"rooms/{id}");

    public async Task<ServiceResponse> AddRoomAsync(RoomCreateRequest roomDto)
        => await PostForResponseAsync("rooms", roomDto);

    public async Task<ServiceResponse> UpdateRoomAsync(RoomUpdateRequest roomDto)
        => await PutForResponseAsync($"rooms/{roomDto.Id}", roomDto);

    public async Task DeleteRoomAsync(int id)
        => await DeleteAsync($"rooms/{id}");
}
