namespace BeHealthy.Application.Services.Interfaces;

public interface IValidatorService
{
    Task ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken);
}
