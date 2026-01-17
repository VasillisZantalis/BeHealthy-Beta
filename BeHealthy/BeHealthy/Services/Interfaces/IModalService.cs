namespace BeHealthy.Services.Interfaces;

public interface IModalService
{
    event Action? OnChange;
    Type? CurrentModal { get; }
    IDictionary<string, object?>? Parameters { get; }
    void Show<T>(IDictionary<string, object?>? parameters = null);
    void Close();
}
