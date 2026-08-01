using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Shared.Dtos.Auth;

public class LoginDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}
