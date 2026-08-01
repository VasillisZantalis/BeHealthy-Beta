using BeHealthy.Shared.Dtos.Common;
using BeHealthy.Frontend.Services;
using BeHealthy.Frontend.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.Components.Pages;

public class BasePage : ComponentBase
{
    public bool IsLoading { get; set; } = false;

    [Inject]
    protected BreadcrumbServiceState Breadcrumbs { get; set; } = default!;

    [Inject]
    protected IModalService ModalService { get; set; } = default!;

    [Inject]
    protected ToastService ToastService { get; set; } = default!;

    [Inject]
    protected AlertModalStateService AlertModalStateService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        Breadcrumbs.ResetBreadcrumb();

        await base.OnInitializedAsync();
    }

    protected bool HandleServiceResponse(ServiceResponse response)
    {
        IsLoading = false;

        if (!response.Success)
        {
            AlertModalStateService.Show(null, response.ErrorMessage);
            return false;
        }
        return true;
    }

}
