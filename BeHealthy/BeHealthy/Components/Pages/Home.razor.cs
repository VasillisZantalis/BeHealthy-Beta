using BeHealthy.Models;
using BeHealthy.Persistance;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public partial class Home : BasePage
{
    [Inject] PrivilegeStateService PrivilegeStateService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        SetBreadcrumbs();
        await PrivilegeStateService.LoadUserPrivileges();
    }

    private void SetBreadcrumbs()
    {
        Breadcrumbs.ResetBreadcrumb();
        Breadcrumbs.AddBreadcrumb(new Breadcrumb
        {
            Text = Resource.Dashboard,
            Link = RoutingEndpoints.HOME_PAGE,
            Active = true
        });
    }
}
