using BeHealthy.Application.Dtos.Room;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class RoomMapper
{
    public static RoomDto MapToDto(this Room room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            Department = room.Department?.Name ?? string.Empty,
            DepartmentId = room.DepartmentId
        };
    }

    public static Room MapToDomain(this RoomDto dto)
    {
        return new Room
        {
            Id = dto.Id,
            Name = dto.Name,
            Number = dto.Number,
        };
    }

    public static Room MapToDomain(this RoomForCreationDto dto)
    {
        return new Room
        {
            Name = dto.Name,
            Number = dto.Number,
            DepartmentId = dto.DepartmentId
        };
    }

    public static Room MapToDomain(this RoomForUpdateDto dto)
    {
        return new Room
        {
            Id = dto.Id,
            Name = dto.Name,
            Number = dto.Number,
            DepartmentId = dto.DepartmentId
        };
    }

    public static RoomForUpdateDto MapToUpdateDto(this Room room)
    {
        return new RoomForUpdateDto
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static RoomDto MapToSelf(this RoomDto room)
    {
        return new RoomDto
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            Department = room.Department
        };
    }

    public static RoomForUpdateDto MapDtoToUpdateDto(this RoomDto room)
    {
        return new RoomForUpdateDto
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static RoomForCreationDto MapDtoToCreateDto(this RoomDto room)
    {
        return new RoomForCreationDto
        {
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static IEnumerable<RoomDto> MapToDto(this IEnumerable<Room> rooms)
    {
        return rooms.Select(room => room.MapToDto());
    }

    public static IEnumerable<Room> MapToDomain(this IEnumerable<RoomDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static ICollection<RoomDto> MapToDto(this ICollection<Room> rooms)
    {
        return rooms.Select(room => room.MapToDto()).ToList();
    }

    public static ICollection<Room> MapToDomain(this ICollection<RoomDto> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
