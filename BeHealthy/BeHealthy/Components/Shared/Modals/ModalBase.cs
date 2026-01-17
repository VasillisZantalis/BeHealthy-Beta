using BeHealthy.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Components.Shared.Modals;

public abstract class ModalBase : ComponentBase
{
    [Inject] protected IModalService ModalService { get; set; } = default!;

    protected void Close() => ModalService.Close();
}
