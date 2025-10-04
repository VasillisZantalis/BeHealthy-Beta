namespace BeHealthy.States;

public class ToastService
{
    private readonly Queue<(string Message, string Type)> _pendingToasts = new();

    public event Action<string, string>? OnShow;

    public void ShowToast(string message, string type = "info")
    {
        OnShow?.Invoke(message, type);
    }

    public void EnqueueToast(string message, string type = "info")
    {
        _pendingToasts.Enqueue((message, type));
    }

    public void FlushToasts()
    {
        while (_pendingToasts.Count > 0)
        {
            var (msg, type) = _pendingToasts.Dequeue();
            ShowToast(msg, type);
        }
    }
}
