using BeHealthy.Application.Dtos.Common;
using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public class BasePage : ComponentBase
{
    [Inject]
    protected LoaderServiceState LoaderService { get; set; } = default!;

    public bool IsLoading { get; set; } = true;

    [Inject]
    protected BreadcrumbServiceState Breadcrumbs { get; set; } = default!;

    [Inject]
    protected ConfirmDeleteStateService ConfirmDeleteService { get; set; } = default!;

    [Inject]
    protected ToastrStateService ToastrStateService { get; set; } = default!;

    [Inject]
    protected AlertModalStateService AlertModalStateService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Breadcrumbs.ResetBreadcrumb();

        await base.OnInitializedAsync();
    }

    protected bool HandleServiceResponse(ServiceResponse response)
    {
        LoaderService.SetLoader(false);

        if (!response.Success)
        {
            AlertModalStateService.Show(null, response.ErrorMessage);
            return false;
        }
        return true;
    }

}
