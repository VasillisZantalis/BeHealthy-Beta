using BeHealthy.Shared.Dtos.Room;
using BeHealthy.Domain.Entities;

namespace BeHealthy.Application.Mappings;

public static class RoomMapper
{
    public static RoomResponse MapToDto(this Room room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            Department = room.Department?.Name ?? string.Empty,
            DepartmentId = room.DepartmentId
        };
    }

    public static Room MapToDomain(this RoomResponse dto)
    {
        return new Room
        {
            Id = dto.Id,
            Name = dto.Name,
            Number = dto.Number,
        };
    }

    public static Room MapToDomain(this RoomCreateRequest dto)
    {
        return new Room
        {
            Name = dto.Name,
            Number = dto.Number,
            DepartmentId = dto.DepartmentId
        };
    }

    public static Room MapToDomain(this RoomUpdateRequest dto)
    {
        return new Room
        {
            Id = dto.Id,
            Name = dto.Name,
            Number = dto.Number,
            DepartmentId = dto.DepartmentId
        };
    }

    public static RoomUpdateRequest MapToUpdateDto(this Room room)
    {
        return new RoomUpdateRequest
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static RoomResponse MapToSelf(this RoomResponse room)
    {
        return new RoomResponse
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            Department = room.Department
        };
    }

    public static RoomUpdateRequest MapDtoToUpdateDto(this RoomResponse room)
    {
        return new RoomUpdateRequest
        {
            Id = room.Id,
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static RoomCreateRequest MapDtoToCreateDto(this RoomResponse room)
    {
        return new RoomCreateRequest
        {
            Name = room.Name,
            Number = room.Number,
            DepartmentId = room.DepartmentId
        };
    }

    public static IEnumerable<RoomResponse> MapToDto(this IEnumerable<Room> rooms)
    {
        return rooms.Select(room => room.MapToDto());
    }

    public static IEnumerable<Room> MapToDomain(this IEnumerable<RoomResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain());
    }

    public static ICollection<RoomResponse> MapToDto(this ICollection<Room> rooms)
    {
        return rooms.Select(room => room.MapToDto()).ToList();
    }

    public static ICollection<Room> MapToDomain(this ICollection<RoomResponse> dtos)
    {
        return dtos.Select(dto => dto.MapToDomain()).ToList();
    }
}
