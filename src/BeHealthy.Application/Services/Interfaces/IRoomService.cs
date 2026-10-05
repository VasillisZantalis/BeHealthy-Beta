using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Application.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomResponse>> GetAllRoomsAsync(CancellationToken cancellationToken = default);
    Task<RoomResponse?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddRoomAsync(RoomCreateRequest roomDto, CancellationToken cancellationToken = default);
    Task<ServiceResponse> UpdateRoomAsync(RoomUpdateRequest roomDto, CancellationToken cancellationToken = default);
    Task DeleteRoomAsync(int id, CancellationToken cancellationToken = default);
}
