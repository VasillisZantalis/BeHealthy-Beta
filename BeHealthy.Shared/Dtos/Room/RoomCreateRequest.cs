using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Dtos.Room;

public class RoomCreateRequest
{
    public required string Name { get; set; }
    public int Number { get; set; }
    public int DepartmentId { get; set; }
}
