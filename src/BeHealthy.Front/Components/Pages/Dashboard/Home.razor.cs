using BeHealthy.Front.Models;
using BeHealthy.Front.Services.CurrentUser;
using BeHealthy.Shared.Locales;
using BeHealthy.Front.States;
using Microsoft.AspNetCore.Components;
using BeHealthy.Front.Extensions;

namespace BeHealthy.Front.Components.Pages.Dashboard;

public partial class Home : BasePage
{
    [Inject] ICurrentUserService CurrentUser { get; set; } = default!;

    private bool isAdminUser = default;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
        isAdminUser = CurrentUser.IsAdmin;
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
