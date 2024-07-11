using AutoMapper;
using BeHealthy.Repositories.Interfaces;
using BeHealthy.Services.Interfaces;
using BeHealthy.Shared.Models.Dtos.Room;
using BeHealthy.Shared.Models.Entities;

namespace BeHealthy.Services;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<RoomDto> GetRoomByIdAsync(int id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        return _mapper.Map<RoomDto>(room);
    }

    public async Task AddRoomAsync(RoomForCreationDto roomDto)
    {
        var room = _mapper.Map<Room>(roomDto);
        await _roomRepository.AddAsync(room);
    }

    public async Task UpdateRoomAsync(RoomForUpdateDto roomDto)
    {
        var room = _mapper.Map<Room>(roomDto);
        await _roomRepository.UpdateAsync(room);
    }

    public async Task DeleteRoomAsync(int id)
    {
        await _roomRepository.DeleteAsync(id);
    }
}
