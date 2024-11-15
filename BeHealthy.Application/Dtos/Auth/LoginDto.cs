using System.ComponentModel.DataAnnotations;

namespace BeHealthy.Application.Dtos.Auth;

public class LoginDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}
