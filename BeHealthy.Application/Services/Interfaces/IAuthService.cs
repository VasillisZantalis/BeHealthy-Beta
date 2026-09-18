using BeHealthy.Shared.Dtos.Auth;

namespace BeHealthy.Application.Services.Interfaces;

public record AuthResult(bool Success, string? ErrorMessage, LoginResponse? Data)
{
    public static AuthResult Ok(LoginResponse data) => new(true, null, data);
    public static AuthResult Failed(string errorMessage) => new(false, errorMessage, null);
}

public interface IAuthService
{
    Task<AuthResult> LoginAsync(LoginRequest request);
}
