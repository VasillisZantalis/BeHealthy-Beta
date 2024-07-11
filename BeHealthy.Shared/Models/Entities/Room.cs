namespace BeHealthy.Shared.Models.Entities;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Number { get; set; }
    public required string Department { get; set; }
}
