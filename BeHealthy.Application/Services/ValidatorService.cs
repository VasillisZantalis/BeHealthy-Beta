using FluentValidation;

namespace BeHealthy.Application.Services;

public class ValidatorService(ServiceProvider serviceProvider) : IValidatorService
{
    public async Task ValidateAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        var validators = serviceProvider
            .GetServices<IValidator<TRequest>>();

        if (!validators.Any())
            return;

        var context = new ValidationContext<TRequest>(request);

        foreach (var validator in validators)
        {
            var result = await validator.ValidateAsync(
                context,
                cancellationToken);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }
    }
}
