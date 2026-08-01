using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Dtos.Room;

public class RoomCreateDto
{
    public required string Name { get; set; }
    public int Number { get; set; }
    public int DepartmentId { get; set; }
}
