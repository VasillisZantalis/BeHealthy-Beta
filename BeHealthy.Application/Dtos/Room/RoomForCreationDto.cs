using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Room;

public class RoomForCreationDto
{
    public required string Name { get; set; }
    public int Number { get; set; }
    public int DepartmentId { get; set; }
}
