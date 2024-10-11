using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Room;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class RoomService : IRoomService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _unitOfWork.RoomRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<RoomDto> GetRoomByIdAsync(int id)
    {
        var room = await _unitOfWork.RoomRepository.GetByIdAsync(id);
        return _mapper.Map<RoomDto>(room);
    }

    public async Task AddRoomAsync(RoomForCreationDto roomDto)
    {
        var room = _mapper.Map<Room>(roomDto);
        await _unitOfWork.RoomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(RoomForUpdateDto roomDto)
    {
        var room = _mapper.Map<Room>(roomDto);
        await _unitOfWork.RoomRepository.UpdateAsync(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _unitOfWork.RoomRepository.DeleteAsync(id);
    }
}
