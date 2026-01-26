namespace BeHealthy.States;

public class ToastService
{
    private static readonly Queue<(string Message, string Type)> _pendingToasts = new();
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    public event Action<string, string>? OnShow;

    public void ShowToast(string message, string type = "info")
    {
        OnShow?.Invoke(message, type);
    }

    public async Task EnqueueToastAsync(string message, string type = "info")
    {
        await _semaphore.WaitAsync();
        try
        {
            _pendingToasts.Enqueue((message, type));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task FlushToastsAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            while (_pendingToasts.Count > 0)
            {
                var (msg, type) = _pendingToasts.Dequeue();
                ShowToast(msg, type);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void EnqueueToast(string message, string type = "info")
    {
        _semaphore.Wait();
        try
        {
            _pendingToasts.Enqueue((message, type));
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void FlushToasts()
    {
        _semaphore.Wait();
        try
        {
            while (_pendingToasts.Count > 0)
            {
                var (msg, type) = _pendingToasts.Dequeue();
                ShowToast(msg, type);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}
