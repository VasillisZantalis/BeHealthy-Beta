using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Room;

public class RoomForCreationDto
{
    [Required]
    public required string Name { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
