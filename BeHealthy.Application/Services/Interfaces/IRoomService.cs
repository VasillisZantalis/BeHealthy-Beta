using BeHealthy.Application.Dtos.Room;

namespace BeHealthy.Application.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomForCreationDto roomDto);
    Task UpdateRoomAsync(int id, RoomForUpdateDto roomDto);
    Task DeleteRoomAsync(int id);
}
