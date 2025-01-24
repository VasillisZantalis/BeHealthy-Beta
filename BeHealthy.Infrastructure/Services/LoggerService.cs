using BeHealthy.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace BeHealthy.Infrastructure.Services;

public class LoggerService<T>(ILogger<T> logger) : ILoggerService<T>
{
    public void LogException(Exception ex, string message) => logger.LogError(ex, message);

    public void LogInformation(string message) => logger.LogInformation(message);

    public void LogWarning(string message) => logger.LogWarning(message);
}
