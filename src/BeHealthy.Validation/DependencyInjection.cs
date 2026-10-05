using Microsoft.Extensions.DependencyInjection;

namespace BeHealthy.Validation;

public static class DependencyInjection
{
    /// <summary>
    /// Registers every validator in this assembly as <c>IValidator&lt;TRequest&gt;</c> (scoped, because
    /// some depend on <see cref="Settings.IValidationSettingsProvider"/>). The host must register its
    /// own <see cref="Settings.IValidationSettingsProvider"/> implementation.
    /// </summary>
    public static IServiceCollection AddSharedValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Settings.IValidationSettingsProvider>(ServiceLifetime.Scoped);

        return services;
    }
}
