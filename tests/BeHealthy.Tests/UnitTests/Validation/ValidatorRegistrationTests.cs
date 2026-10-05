using BeHealthy.Application;
using BeHealthy.Application.Validators.Doctors;
using BeHealthy.Shared.Parameters;
using BeHealthy.Validation;
using BeHealthy.Validation.Doctors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BeHealthy.Tests.UnitTests.Validation;

public class ValidatorRegistrationTests
{
    /// <summary>
    /// Every request the API accepts must be validated. This fails when a new *Request DTO or
    /// QueryParameters type is added without a validator in BeHealthy.Validation.
    /// </summary>
    [Fact]
    public void AddSharedValidators_EveryRequestAndQueryParametersType_HasAValidator()
    {
        // Arrange
        var services = new ServiceCollection().AddSharedValidators();
        var requestTypes = typeof(ServiceResponse).Assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => (t.Namespace?.StartsWith("BeHealthy.Shared.Dtos") == true && t.Name.EndsWith("Request"))
                || typeof(QueryParameters).IsAssignableFrom(t))
            .ToList();

        // Act
        var missing = requestTypes
            .Where(t => !services.Any(d => d.ServiceType == typeof(IValidator<>).MakeGenericType(t)))
            .Select(t => t.Name)
            .ToList();

        // Assert
        requestTypes.ShouldNotBeEmpty();
        missing.ShouldBeEmpty();
    }

    [Fact]
    public void AddApplication_RegistersSharedAndServerValidators_ForTheSameRequest()
    {
        // Arrange
        var services = new ServiceCollection().AddApplication();

        // Act
        var implementations = services
            .Where(d => d.ServiceType == typeof(IValidator<DoctorCreateRequest>))
            .Select(d => d.ImplementationType)
            .ToList();

        // Assert
        implementations.ShouldBe(new[] { typeof(DoctorCreateRequestValidator), typeof(DoctorCreateRequestServerValidator) }, ignoreOrder: true);
        services.ShouldAllBe(d => d.ServiceType != typeof(IValidator<DoctorCreateRequest>) || d.Lifetime == ServiceLifetime.Scoped);
    }
}
