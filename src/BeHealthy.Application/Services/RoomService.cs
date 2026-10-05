using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Shared.Locales;

namespace BeHealthy.Application.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;

    public RoomService(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomResponse>> GetAllRoomsAsync(CancellationToken cancellationToken = default)
    {
        var rooms = await _roomRepository.GetAllRoomsAsync(cancellationToken);
        return rooms.MapToDto();
    }

    public async Task<RoomResponse?> GetRoomByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetRoomByIdAsync(id, cancellationToken);
        return room?.MapToDto();
    }

    public async Task AddRoomAsync(RoomCreateRequest roomDto, CancellationToken cancellationToken = default)
    {
        var room = roomDto.MapToDomain();
        await _roomRepository.AddAsync(room, cancellationToken);
        await _roomRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ServiceResponse> UpdateRoomAsync(RoomUpdateRequest roomDto, CancellationToken cancellationToken = default)
    {
        var room = await _roomRepository.GetByIdAsync(roomDto.Id, cancellationToken);
        if (room is null)
        {
            return ServiceResponse.Failed(Resource.NotFound);
        }

        room.Name = roomDto.Name;
        room.Number = roomDto.Number;
        room.DepartmentId = roomDto.DepartmentId;

        await _roomRepository.UpdateAsync(room);
        await _roomRepository.SaveChangesAsync(cancellationToken);
        return ServiceResponse.Successful();
    }

    public async Task DeleteRoomAsync(int id, CancellationToken cancellationToken = default)
    {
        if (await _roomRepository.DeleteAsync(id, cancellationToken))
        {
            await _roomRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
