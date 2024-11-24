using BeHealthy.States;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Pages;

public class BasePage : ComponentBase
{
    [Inject]
    protected LoaderServiceState LoaderService { get; set; } = default!;

    public bool IsLoading { get; set; } = true;
}
