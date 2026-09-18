using BeHealthy.Front.Models;
using BeHealthy.Front.Services.CurrentUser;
using BeHealthy.Shared.Locales;
using BeHealthy.Front.States;
using Microsoft.AspNetCore.Components;
using BeHealthy.Front.Extensions;
using BeHealthy.Front.Services.Interfaces;
using BeHealthy.Front.Components.Shared.Modals;

namespace BeHealthy.Front.Components.Pages.Dashboard;

public partial class Home : BasePage
{
    [Inject] ICurrentUserService CurrentUser { get; set; } = default!;
    [Inject] ISeedingService seedingService { get; set; } = default!;
    [Inject] NavigationManager NavigationManager { get; set; } = default!;

    private bool isAdminUser = default;

    protected override void OnInitialized()
    {
        SetBreadcrumbs();
    }

    protected override async Task OnInitializedAsync()
    {
        isAdminUser = CurrentUser.IsAdmin;

        await CheckAndPromptSeeding();
    }

    private async Task CheckAndPromptSeeding()
    {
        IsLoading = true;

        var needsSeeding = await seedingService.NeedsSeedingAsync();
        
        if (needsSeeding)
        {
            ModalService.Show<SeedingModal>(
                new Dictionary<string, object?>
                {
                    { nameof(SeedingModal.OnSeedingCompleted), EventCallback.Factory.Create(this, OnSeedingCompleted) }
                });
        }

        IsLoading = false;
    }

    private void OnSeedingCompleted()
    {
        NavigationManager.Refresh(true);
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
