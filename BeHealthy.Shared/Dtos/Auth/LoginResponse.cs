namespace BeHealthy.Shared.Dtos.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAtUtc { get; set; }
    public UserResponse User { get; set; } = new();
}
