using BeHealthy.Shared.Dtos.Auth;

namespace BeHealthy.API.Controllers;

/// <summary>Issues JWT access tokens for the Front app's cookie-based sign-in.</summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController(IAuthService authService) : ApiControllerBase
{
    /// <summary>Validates credentials and returns a signed JWT plus the user's profile/role.</summary>
    [HttpPost("login")]
    [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var result = await authService.LoginAsync(request);
        if (!result.Success)
            return Unauthorized(new { message = result.ErrorMessage });

        return Ok(result.Data);
    }
}
