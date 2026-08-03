using BeHealthy.Shared.Dtos.Room;

namespace BeHealthy.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomResponse>> GetAllRoomsAsync()
    {
        var rooms = await _unitOfWork.RoomRepository.GetAllRoomsAsync();
        return rooms.MapToDto();
    }

    public async Task<RoomResponse?> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);
        return room?.MapToDto();
    }

    public async Task AddRoomAsync(RoomCreateRequest roomDto)
    {
        var room = roomDto.MapToDomain();
        await _unitOfWork.RoomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(RoomUpdateRequest roomDto)
    {
        var room = roomDto.MapToDomain();
        await _unitOfWork.RoomRepository.UpdateAsync(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }
}
