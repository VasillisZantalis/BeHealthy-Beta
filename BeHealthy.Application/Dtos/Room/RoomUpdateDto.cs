using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Room;

public class RoomUpdateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Number { get; set; }
    public int DepartmentId { get; set; }
}
