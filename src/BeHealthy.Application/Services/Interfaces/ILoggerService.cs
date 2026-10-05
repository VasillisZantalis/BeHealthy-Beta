namespace BeHealthy.Application.Services.Interfaces;

public interface ILoggerService<T>
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogException(Exception ex, string message);
}
