using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;

namespace BeHealthy.Components.Pages;

public partial class Home : BasePage
{
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
            Link = string.Empty,
            Active = true
        });
    }
}
