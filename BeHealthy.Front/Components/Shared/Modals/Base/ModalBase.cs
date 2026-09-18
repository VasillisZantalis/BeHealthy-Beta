using BeHealthy.Front.Services;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.Front.Components.Shared.Modals.Base;

public abstract class ModalBase : ComponentBase
{
    [Inject] protected IModalService ModalService { get; set; } = default!;

    protected void Close() => ModalService.Close();
}
