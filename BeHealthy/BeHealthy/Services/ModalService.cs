using BeHealthy.Services.Interfaces;

namespace BeHealthy.Services;

public class ModalService : IModalService
{
    public event Action? OnChange;

    public Type? CurrentModal { get; private set; }
    public IDictionary<string, object?>? Parameters { get; private set; }

    public void Show<T>(IDictionary<string, object?>? parameters = null)
    {
        CurrentModal = typeof(T);
        Parameters = parameters;
        NotifyStateChanged();
    }

    public void Close()
    {
        CurrentModal = null;
        Parameters = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
        => OnChange?.Invoke();
}
