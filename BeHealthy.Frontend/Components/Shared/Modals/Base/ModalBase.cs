using BeHealthy.Frontend.Services;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.Components.Shared.Modals.Base;

public abstract class ModalBase : ComponentBase
{
    [Inject] protected IModalService ModalService { get; set; } = default!;

    protected void Close() => ModalService.Close();
}
