using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Shared.Locales;

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
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ServiceResponse> UpdateRoomAsync(RoomUpdateRequest roomDto)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(roomDto.Id);
        if (room is null)
            return ServiceResponse.Failed(Resource.NotFound);

        room.Name = roomDto.Name;
        room.Number = roomDto.Number;
        room.DepartmentId = roomDto.DepartmentId;

        await _unitOfWork.RoomRepository.UpdateAsync(room);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Successful();
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }
}
