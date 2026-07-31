namespace BeHealthy.Frontend.States;

public class NavMenuState
{
    public event Action? OnChange;

    private bool _isVisible = true;

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                NotifyStateChanged();
            }
        }
    }

    public void Toggle() => IsVisible = !IsVisible;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
