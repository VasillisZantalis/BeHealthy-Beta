using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Frontend.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomCreateDto roomDto);
    Task UpdateRoomAsync(RoomUpdateDto roomDto);
    Task DeleteRoomAsync(int id);
}
