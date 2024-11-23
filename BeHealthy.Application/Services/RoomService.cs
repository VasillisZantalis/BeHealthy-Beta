using BeHealthy.Application.Dtos.Room;
using BeHealthy.Application.Mappings;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Domain.Interfaces;

namespace BeHealthy.Application.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _unitOfWork.RoomRepository.GetAllRoomsAsync();
        return rooms.MapToDto();
    }

    public async Task<RoomDto> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);
        return room.MapToDto();
    }

    public async Task AddRoomAsync(RoomForCreationDto roomDto)
    {
        var room = roomDto.MapToDomain();
        await _unitOfWork.RoomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(int id, RoomForUpdateDto roomDto)
    {
        var room = roomDto.MapToDomain();
        await _unitOfWork.RoomRepository.UpdateAsync(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }
}
