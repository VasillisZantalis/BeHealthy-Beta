using BeHealthy.Models;
using BeHealthy.Shared.Locales;
using BeHealthy.States;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using BeHealthy.Application.Extensions;
using BeHealthy.Application.Services.Interfaces;
using BeHealthy.Components.Shared.Modals;

namespace BeHealthy.Components.Pages.Dashboard;

public partial class Home : BasePage
{
    [Inject] AuthenticationStateProvider authenticationStateProvider { get; set; } = default!;
    [Inject] ISeedingService seedingService { get; set; } = default!;
    [Inject] NavigationManager navigationManager { get; set; } = default!;

    private bool isAdminUser = default;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        isAdminUser = authState.User.IsAdminUser();

        await CheckAndPromptSeeding();
    }

    private async Task CheckAndPromptSeeding()
    {
        LoaderService.SetLoader(true);

        var needsSeeding = await seedingService.NeedsSeedingAsync();
        
        if (needsSeeding)
        {
            ModalService.Show<SeedingModal>(
                new Dictionary<string, object?>
                {
                    { nameof(SeedingModal.OnSeedingCompleted), EventCallback.Factory.Create(this, OnSeedingCompleted) }
                });
        }

        LoaderService.SetLoader(false);
    }

    private void OnSeedingCompleted()
    {
        navigationManager.Refresh(true);
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
