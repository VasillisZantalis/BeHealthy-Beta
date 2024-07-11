using BeHealthy.Shared.Models.Dtos.Room;

namespace BeHealthy.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomForCreationDto roomDto);
    Task UpdateRoomAsync(RoomForUpdateDto roomDto);
    Task DeleteRoomAsync(int id);
}
