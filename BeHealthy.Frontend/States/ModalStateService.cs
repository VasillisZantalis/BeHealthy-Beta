using Microsoft.AspNetCore.Components;

namespace BeHealthy.Frontend.States;

public class ModalStateService
{
    public event Action? OnShow;
    public event Action? OnHide;
    public string Title { get; private set; } = string.Empty;
    public RenderFragment? ChildContent { get; private set; }
    public bool IsForDelete { get; private set; }

    public void Show(string title, RenderFragment content, bool isForDelete = false)
    {
        Title = title;
        ChildContent = content;
        IsForDelete = isForDelete;
        OnShow?.Invoke();
    }

    public void Hide()
    {
        OnHide?.Invoke();
    }
}
