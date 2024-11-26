using Microsoft.AspNetCore.Components;

namespace BeHealthy.States;

public class ConfirmDeleteStateService
{

    private readonly NavigationManager _navigationManager;

    public ConfirmDeleteStateService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public event Action<Func<Task>>? OnRequestDelete;
    public event Action? OnCancel;

    public void RequestDelete(Func<Task> onConfirm)
    {
        OnRequestDelete?.Invoke(onConfirm);
    }

    public void Cancel()
    {
        OnCancel?.Invoke();
    }
}
