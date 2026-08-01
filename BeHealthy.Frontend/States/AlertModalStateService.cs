using BeHealthy.Shared.Locales;

namespace BeHealthy.Frontend.States;

public class AlertModalStateService
{
    public event Action? OnShow;
    public event Action? OnHide;
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool Success { get; private set; }

    public void Show(string? title, string? message, bool success = false)
    {
        Title = title ?? Resource.Error;
        Message = message ?? Resource.SomethingWentWrong;
        Success = success;
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
        Title = string.Empty;
        Message = string.Empty;
        Success = false;
    }
}
