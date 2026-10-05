using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BeHealthy.Front.States;

/// <summary>
/// Per-circuit toast notifications. Toasts that must survive a full page reload are kept in the
/// browser tab's protected session storage, so they never reach another user's circuit.
/// </summary>
public class ToastService(ProtectedSessionStorage sessionStorage)
{
    private const string PendingToastsKey = "pending-toasts";

    public event Action<string, string>? OnShow;

    public void ShowToast(string message, string type = "info")
    {
        OnShow?.Invoke(message, type);
    }

    /// <summary>Stores a toast to be shown after the next full page load (e.g. after NavigationManager.Refresh(true)).</summary>
    public async Task EnqueueToastAsync(string message, string type = "info")
    {
        var pendingToasts = await GetPendingToastsAsync();
        pendingToasts.Add(new PendingToast(message, type));
        await sessionStorage.SetAsync(PendingToastsKey, pendingToasts);
    }

    public async Task FlushToastsAsync()
    {
        var pendingToasts = await GetPendingToastsAsync();
        if (pendingToasts.Count == 0)
        {
            return;
        }

        await sessionStorage.DeleteAsync(PendingToastsKey);

        foreach (var toast in pendingToasts)
        {
            ShowToast(toast.Message, toast.Type);
        }
    }

    private async Task<List<PendingToast>> GetPendingToastsAsync()
    {
        try
        {
            var result = await sessionStorage.GetAsync<List<PendingToast>>(PendingToastsKey);
            return result.Success && result.Value is not null ? result.Value : [];
        }
        catch (CryptographicException)
        {
            // Stored with a data protection key that no longer exists; drop it.
            await sessionStorage.DeleteAsync(PendingToastsKey);
            return [];
        }
    }

    private sealed record PendingToast(string Message, string Type);
}
