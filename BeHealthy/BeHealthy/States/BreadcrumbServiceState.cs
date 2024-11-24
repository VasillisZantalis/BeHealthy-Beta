using BeHealthy.Models;
using Microsoft.AspNetCore.Components;

namespace BeHealthy.States;

public class BreadcrumbServiceState
{
    private readonly NavigationManager _navigationManager;

    public BreadcrumbServiceState(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
    }

    public List<Breadcrumb> Breadcrumbs { get; set; } = new();
    public bool IsBreadcrumbVisible { get; set; }
    public bool IsBackButtonVisible { get; set; }
    public event Action OnChange;

    public void ResetBreadcrumb()
    {
        Breadcrumbs = new List<Breadcrumb>();
        IsBackButtonVisible = false;
        IsBreadcrumbVisible = false;
    }

    public void SetBreadcrumbs(List<Breadcrumb> breadcrumbs)
    {
        if (breadcrumbs == null || !breadcrumbs.Any())
        {
            ResetBreadcrumb();
            return;
        }

        Breadcrumbs = breadcrumbs;
        IsBreadcrumbVisible = true;
        IsBackButtonVisible = Breadcrumbs.Count > 1;
        NotifyStateChanged();
    }

    public void AddBreadcrumb(Breadcrumb breadcrumb)
    {
        if (breadcrumb == null) return;

        Breadcrumbs.Add(breadcrumb);
        IsBreadcrumbVisible = true;
        IsBackButtonVisible = Breadcrumbs.Count > 1;
        NotifyStateChanged();
    }

    public void BackButtonClickHandler()
    {
        if (!Breadcrumbs.Any()) return;

        var lastInactiveBreadcrumb = Breadcrumbs
            .Where(b => !string.IsNullOrEmpty(b.Link))
            .Reverse()
            .FirstOrDefault();

        if (lastInactiveBreadcrumb != null)
        {
            _navigationManager.NavigateTo(lastInactiveBreadcrumb.Link);
        }
        else
        {
            _navigationManager.NavigateTo("/");
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
