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
    [Inject] AuthenticationStateProvider _authenticationStateProvider { get; set; } = default!;
    [Inject] ISeedingService _seedingService { get; set; } = default!;

    private bool _isAdminUser = default;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        _isAdminUser = authState.User.IsAdminUser();

        await CheckAndPromptSeeding();
    }

    private async Task CheckAndPromptSeeding()
    {
        LoaderService.SetLoader(true);

        var needsSeeding = await _seedingService.NeedsSeedingAsync();
        
        if (needsSeeding)
        {
            await Task.Delay(500); // Small delay for better UX
            
            ModalService.Show<SeedingModal>(
                new Dictionary<string, object?>
                {
                    { nameof(SeedingModal.OnSeedingCompleted), EventCallback.Factory.Create(this, OnSeedingCompleted) }
                });
        }

        LoaderService.SetLoader(false);
    }

    private async Task OnSeedingCompleted()
    {
        StateHasChanged();
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
