using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BeHealthy.Front.Services.Auth;

/// <summary>
/// Surfaces the cookie-authenticated <see cref="ClaimsPrincipal"/> to Blazor's authorization
/// components (AuthorizeView, CascadingAuthenticationState). Captured once per circuit for the
/// same reason as <see cref="CurrentUser.CurrentUserService"/>: HttpContext is only live during
/// the initial request that starts the circuit.
/// </summary>
public class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _authenticationState;

    public ServerAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
        _authenticationState = new AuthenticationState(user);
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
        => Task.FromResult(_authenticationState);
}
