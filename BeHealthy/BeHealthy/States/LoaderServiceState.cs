namespace BeHealthy.States;

public class LoaderServiceState
{
    public event Action<bool>? OnChange;

    public void SetLoader(bool isLoading)
    {
        OnChange?.Invoke(isLoading);
    }
}
