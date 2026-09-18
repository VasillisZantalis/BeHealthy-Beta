using BeHealthy.Front.Common;
using BeHealthy.Front.Models;

namespace BeHealthy.Front.States;

public class ToastrStateService
{
    public event Func<ToastrNotification, Task>? OnShow;

    public async Task ShowSuccess(string message, int? duration = null)
    {
        if (OnShow != null)
        {
            await OnShow.Invoke(new ToastrNotification
            {
                Message = message,
                Severity = Severity.Success,
                Icon = FAIcon.CircleCheck,
                Duration = duration
            });
        }
    }

    public async Task ShowFailed(string message, int? duration = null)
    {
        if (OnShow != null)
        {
            await OnShow.Invoke(new ToastrNotification
            {
                Message = message,
                Severity = Severity.Danger,
                Icon = FAIcon.CircleXmark,
                Duration = duration
            });
        }
    }

    public async Task ShowInfo(string message, int? duration = null)
    {
        if (OnShow != null)
        {
            await OnShow.Invoke(new ToastrNotification
            {
                Message = message,
                Severity = Severity.Info,
                Icon = FAIcon.CircleInfo,
                Duration = duration
            });
        }
    }

    public async Task ShowWarning(string message, int? duration = null)
    {
        if (OnShow != null)
        {
            await OnShow.Invoke(new ToastrNotification
            {
                Message = message,
                Severity = Severity.Warning,
                Icon = FAIcon.TriangleExclamation,
                Duration = duration
            });
        }
    }
}
