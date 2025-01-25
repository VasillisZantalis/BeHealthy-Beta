using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BeHealthy.Application.Extensions;

namespace BeHealthy.Components.Pages.Dashboard;

public partial class Home : BasePage
{
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;

    private bool _isAdminUser = default;

    protected override async Task OnInitializedAsync()
    {
        SetBreadcrumbs();
        await PrivilegeStateService.LoadUserPrivileges();

        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _isAdminUser = authState.User.IsAdminUser();
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.ResetBreadcrumb();
        Breadcrumbs.AddBreadcrumb(new Breadcrumb
        {
            Text = Resource.Dashboard,
            Link = string.Empty,
            Active = true
        });
    }
}
