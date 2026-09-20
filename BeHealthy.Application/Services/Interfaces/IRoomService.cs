using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Application.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomResponse>> GetAllRoomsAsync();
    Task<RoomResponse?> GetRoomByIdAsync(int id);
    Task AddRoomAsync(RoomCreateRequest roomDto);
    Task<ServiceResponse> UpdateRoomAsync(RoomUpdateRequest roomDto);
    Task DeleteRoomAsync(int id);
}
