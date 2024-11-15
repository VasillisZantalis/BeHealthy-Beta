using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Room;

public class RoomForUpdateDto
{
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
