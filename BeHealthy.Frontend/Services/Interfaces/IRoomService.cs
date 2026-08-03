using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomResponse>> GetAllRoomsAsync();
    Task<RoomResponse?> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomCreateRequest roomDto);
    Task UpdateRoomAsync(RoomUpdateRequest roomDto);
    Task DeleteRoomAsync(int id);
}
