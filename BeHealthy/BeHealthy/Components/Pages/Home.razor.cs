using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public partial class Home
{
    [Inject] PrivilegeStateService PrivilegeStateService { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await PrivilegeStateService.LoadUserPrivileges();
    }
}
