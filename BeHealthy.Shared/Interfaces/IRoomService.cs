using BeHealthy.Shared.Models.Dtos.Room;

namespace BeHealthy.Shared.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomForCreationDto roomDto);
    Task UpdateRoomAsync(int id, RoomForUpdateDto roomDto);
    Task DeleteRoomAsync(int id);
}
