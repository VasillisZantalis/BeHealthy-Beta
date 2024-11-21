using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Room;

public class RoomForUpdateDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Number { get; set; }
    public string Department { get; set; } = string.Empty;
}
