using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public class BasePage : ComponentBase
{
    [Inject]
    protected LoaderServiceState LoaderService { get; set; } = default!;

    public bool IsLoading { get; set; } = true;

    [Inject] 
    protected PrivilegeStateService PrivilegeStateService { get; set; } = default!;

    [Inject]
    protected BreadcrumbServiceState Breadcrumbs { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Breadcrumbs.ResetBreadcrumb();

        await base.OnInitializedAsync();
    }
}
